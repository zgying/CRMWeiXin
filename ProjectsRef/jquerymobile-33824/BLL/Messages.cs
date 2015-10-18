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
    #region Namespace

    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Configuration;
    using System.Web;
    using System.Web.Configuration;
    using System.Net.Mail;
    using System.Linq;

    #endregion

    /// <summary>
    /// Represents an utility class to send email and / or messages.
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// Utility to send email.
        /// </summary>
        /// <param name="toEmailAddress">Email address of the receiver.</param>
        /// <param name="mailSubject">Email subject.</param>
        /// <param name="mailBody">Email body.</param>
        /// <param name="isHtml">Indicates value if the email is in HTML format or plain text.</param>
        /// <returns>Returns email send result.</returns>
        public static bool SendMail(string toEmailAddress, string mailSubject, string mailBody, bool isHtml = false)
        {
            var mailSendResult = false;
            try
            {
                var webConfiguration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                var mailSettingsSectionGroup = (MailSettingsSectionGroup)webConfiguration.GetSectionGroup(BllConstants.WebConfigurationMailSectionGroupNodeName);
                if (null != mailSettingsSectionGroup)
                {
                    var fromEmailAddress = mailSettingsSectionGroup.Smtp.Network.UserName;
                    if (!string.IsNullOrWhiteSpace(fromEmailAddress) && !string.IsNullOrWhiteSpace(toEmailAddress) && !string.IsNullOrWhiteSpace(mailSubject) && !string.IsNullOrWhiteSpace(mailBody))
                    {
                        using (var mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(fromEmailAddress);
                            toEmailAddress.Split(BllConstants.Semicolon).ToList().ForEach(mailMessage.To.Add);
                            mailMessage.Subject = mailSubject;
                            mailMessage.IsBodyHtml = isHtml;
                            mailMessage.Body = mailBody;
                            using (var smtpClient = new SmtpClient())
                            {
                                var mailConfigurationValid = true;
                                if (mailMessage.To.Any(mailAddress => mailAddress.Address.Equals(mailMessage.From.Address)))
                                {
                                    var smtpRelayHost = ConfigurationManager.AppSettings.Get(BllConstants.SmtpRelayHostAppConfigSettingKey);
                                    int smtpRelayPort;
                                    if (!string.IsNullOrEmpty(smtpRelayHost) && int.TryParse(ConfigurationManager.AppSettings.Get(BllConstants.SmtpRelayPortAppConfigSettingKey), out smtpRelayPort))
                                    {
                                        smtpClient.Host = smtpRelayHost;
                                        smtpClient.Port = smtpRelayPort;
                                        smtpClient.EnableSsl = bool.Parse(ConfigurationManager.AppSettings.Get(BllConstants.SmtpRelayEnableSslAppConfigSettingKey));
                                    }
                                    else
                                    {
                                        var applicationException = new ApplicationException(BllConstants.MailRelaySettingsNotFound);
                                        applicationException.LogToElmah();
                                        mailConfigurationValid = false;
                                    }
                                }

                                if (mailConfigurationValid)
                                {
                                    smtpClient.Send(mailMessage);
                                    mailSendResult = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var applicationExcpetion = new ApplicationException(BllConstants.MailSettingsGroupNotFound);
                    applicationExcpetion.LogToElmah();
                }
            }
            catch (ArgumentException argumentException)
            {
                argumentException.LogToElmah();
            }
            catch (FormatException formatException)
            {
                formatException.LogToElmah();
            }
            catch (InvalidOperationException invalidOperationException)
            {
                invalidOperationException.LogToElmah();
            }
            catch (SmtpFailedRecipientsException smtpFailedRecipientsException)
            {
                smtpFailedRecipientsException.LogToElmah();
            }
            catch (SmtpException smtpException)
            {
                smtpException.LogToElmah();
            }

            return mailSendResult;
        }
    }
}
