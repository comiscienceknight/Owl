﻿using OwlWindowsPhoneApp.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlWindowsPhoneApp.ViewModel
{
    public class EnterIntoChatMessage
    {
        public ChatEntry Message { get; set; }

        public EnterIntoChatMessage()
         :this(null)
        {

        }

        public EnterIntoChatMessage(ChatEntry message)
        {
            Message = message;
        }
    }
}