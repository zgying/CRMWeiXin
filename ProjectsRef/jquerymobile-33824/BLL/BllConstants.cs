/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 */

namespace BLL
{
    /// <summary>
    /// Contains definition of common contants of BLL library.
    /// </summary>
    public static class BllConstants
    {
        /// <summary>
        /// Semicolon character constant.
        /// </summary>
        public const char Semicolon = ';';

        /// <summary>
        /// Web configuration mail section group node name.
        /// </summary>
        public const string WebConfigurationMailSectionGroupNodeName = "system.net/mailSettings";

        /// <summary>
        /// Application configuration key for SMTP relay host.
        /// </summary>
        public const string SmtpRelayHostAppConfigSettingKey = "SMTPRelayHost";

        /// <summary>
        /// Application configuration key for SMTP relay port.
        /// </summary>
        public const string SmtpRelayPortAppConfigSettingKey = "SMTPRelayPort";

        /// <summary>
        /// Application configuration key for SMTP enable SSL.
        /// </summary>
        public const string SmtpRelayEnableSslAppConfigSettingKey = "SMTPRelayEnableSsl";

        /// <summary>
        /// Mail settings group was not found in Web.Config of application.
        /// </summary>
        public const string MailSettingsGroupNotFound = "Mail settings section group not found in configuration.";

        /// <summary>
        /// Mail relay settings not found or is not valid.
        /// </summary>
        public const string MailRelaySettingsNotFound = "Mail relay settings section not found or is not valid.";
    }
}
