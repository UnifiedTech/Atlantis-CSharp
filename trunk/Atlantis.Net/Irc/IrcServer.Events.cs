/*
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * The Initial Developer of the Original Code is Unified Technologies.
 * Copyright (C) 2010 Unified Technologies. All Rights Reserved.
 * 
 * Contributor(s): Zack "Genesis2001" Loveless, Benjamin "aca20031" Buzbee.
 */

namespace Atlantis.Net.Irc
{
    using Atlantis.Net.Irc.Data;
    using System;

    public partial class IrcServer
    {
        #region Methods

        protected virtual void OnNickDetected(String[] parts)
        {


            /*
             * -06:09:05- DEBUG nick -> NICK OperServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Operator Server
             * -06:09:05- DEBUG nick -> NICK NickServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Nickname Server
             * -06:09:05- DEBUG nick -> NICK ChanServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Channel Server
             * -06:09:05- DEBUG nick -> NICK HostServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :vHost Server
             * -06:09:05- DEBUG nick -> NICK MemoServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Memo Server
             * -06:09:05- DEBUG nick -> NICK BotServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Bot Server
             * -06:09:05- DEBUG nick -> NICK HelpServ 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Help Server
             * -06:09:05- DEBUG nick -> NICK Global 2 1324341431 services unifiedtech.org services.unifiedtech.org 0 :Global Noticer
             * -06:09:05- DEBUG nick -> NICK UnifiedTech 2 1324341431 irc unfiiedtech.org services.unifiedtech.org 0 :UnifiedTech.org
             * 
             * -06:09:05- DEBUG nick -> NICK CIA-1 1 1328655612 ~CIA 204.152.223.100 irc.unifiedtech.org 1 :CIA Bot (http://cia.vc)
             * -06:09:05- DEBUG nick -> NICK SniperFodder 1 1328858574 ~UnifiedFo mail.silicateillusion.org irc.unifiedtech.org 1328858574 :Debra FromIT
             * -06:09:05- DEBUG nick -> NICK Lone0001 1 1329005321 ford CPE00240143cbdd-CM602ad06c64f7.cpe.net.cable.rogers.com irc.unifiedtech.org 1329005321 :Ford
             * -06:09:05- DEBUG nick -> NICK Lone 1 1329007296 lone0001 ares.cncfps.com irc.unifiedtech.org 1 :Ford
             */
        }

        #endregion
    }
}