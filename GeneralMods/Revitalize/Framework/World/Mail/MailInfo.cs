using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.World.Objects.Items.Utilities;

namespace Omegasis.Revitalize.Framework.World.Mail
{
    /// <summary>
    /// Used to represent mail information in .json format.
    /// </summary>
    public class MailInfo
    {
        /// <summary>
        /// The title of the mail to be displayed.
        /// </summary>
        public string mailTitle;
        /// <summary>
        /// The message in the mail to be displayed.
        /// </summary>
        public string message;

        /// <summary>
        /// The potential item to be given when this mail is received? Currently unused.
        /// </summary>
        public ItemReference itemReference;

        public MailInfo()
        {

        }

        public MailInfo(string MailTitle, string Message)
        {
            this.mailTitle = MailTitle;
            this.message = Message;
        }

        public MailInfo(string MailTitle, string Message, ItemReference ItemReference)
        {
            this.mailTitle = MailTitle;
            this.message = Message;
            this.itemReference = ItemReference;
        }

    }
}
