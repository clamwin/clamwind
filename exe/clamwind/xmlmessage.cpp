//-----------------------------------------------------------------------------
// Name:        xmlmessage.cpp
// Product:     ClamWin Antivirus
//
// Author(s):      alch [alch at users dot sourceforge dot net]
//                 sherpya [sherpya at users dot sourceforge dot net]
//
// Created:     2006/01/20
// Copyright:   Copyright ClamWin Pty Ltd (c) 2005-2006
// Licence:
//   This program is free software; you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation; either version 2 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program; if not, write to the Free Software
//   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA

#include <clamwind.h>
#include <cwxml.h>

CwXmlMessage::CwXmlMessage(char *buffer, int len, uint32_t type)
{
    this->action = 0;
    this->type = type;
    this->tag = 0;
    this->inside = false;
    this->valid = false;
    this->filename = NULL;
    this->client = INVALID_HANDLE_VALUE;
    this->parser = XML_ParserCreate("UTF-8");
    XML_SetUserData(this->parser, this);
    XML_SetElementHandler(this->parser, startElement, endElement);
    XML_SetCharacterDataHandler(this->parser, dataElement);

    if (XML_Parse(this->parser, buffer, len, 1) != XML_STATUS_ERROR)
        this->Validate();
    else
    {
        /*  FIXME add output in the message */
        char out[512];
        strcpy(out, XML_ErrorString(XML_GetErrorCode(this->parser)));
        dbgprint(LOG_ALWAYS, L"Expat parsing error at line %d\n", XML_GetCurrentLineNumber(this->parser));
    }
    XML_ParserFree(this->parser);
}

CwXmlMessage::~CwXmlMessage(void)
{
    std::map<uint32_t, char *>::iterator i;
    for (i = this->arguments.begin(); i != this->arguments.end(); i++)
        delete i->second;
    this->arguments.clear(); /* TODO: look if it needs the swap hack to free memory */
}

void CwXmlMessage::Validate(void)
{
    std::map<uint32_t, char *>::iterator i;
    dbgprint(LOG_INFO, L"Message parsed -> Action: %s (0x%08x)\n", hash_to_action(this->action), this->action);
    for (i = this->arguments.begin(); i != this->arguments.end(); i++)
    {
        /* FIXME: only for debug very ugly remove :) */
        int len = MultiByteToWideChar(CP_UTF8, 0, i->second, -1, NULL, 0);
        wchar_t *value = new wchar_t[len];
        MultiByteToWideChar(CP_UTF8, 0, i->second, -1, value, len);
        dbgprint(LOG_INFO, L"Found argument %s (0x%08x) - Value: %s\n", hash_to_element(i->first), i->first, value);
        delete value;
    }

    /* Special Case since we need it as wchar_t */
    char *file = this->GetArgument(TAG_FILENAME);
    if (file)
    {
        int len = MultiByteToWideChar(CP_UTF8, 0, file, -1, NULL, 0);
        if (len)
        {
            this->filename = new wchar_t[len];
            MultiByteToWideChar(CP_UTF8, 0, file, -1, this->filename, len);
        }
        else
            dbgprint(LOG_ALWAYS, L"Error in filename conversion %d\n", GetLastError());
    }
    /* TODO Message Validation */
    this->valid = true;
}

char *CwXmlMessage::GetArgument(uint32_t arg)
{
    if (this->arguments.find(arg) == this->arguments.end()) return NULL;
    return this->arguments[arg];
}

void XMLCALL CwXmlMessage::startElement(void *userData, const char *name, const char **atts)
{
    CwXmlMessage *pThis = static_cast<CwXmlMessage *>(userData);
    pThis->tag = hash_string(name);
    if (pThis->tag == pThis->type) pThis->inside = true;
}

void XMLCALL CwXmlMessage::endElement(void *userData, const char *name)
{
    CwXmlMessage *pThis = static_cast<CwXmlMessage *>(userData);
    pThis->tag = 0;
}

void XMLCALL CwXmlMessage::dataElement(void *userData, const XML_Char *s, int len)
{
    CwXmlMessage *pThis = static_cast<CwXmlMessage *>(userData);
    if (!pThis->tag) return;

    if (!pThis->inside)
    {
        dbgprint(LOG_ALWAYS, L"Incorrect message type\n");
        XML_StopParser(pThis->parser, 0);
        return;
    }

    char *value = new char[len + 1];
    memcpy(value, s, len);
    value[len] = 0;

    switch(pThis->tag)
    {
        case TAG_ACTION:
            pThis->action = hash_string(value);
            delete value;
            break;
        default:
            pThis->arguments[pThis->tag] = value;
    }

}
