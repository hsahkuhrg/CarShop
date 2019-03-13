using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using WorkWithData.Models;
using System.Threading.Tasks;

namespace WorkWithData.AccessToData
{
    public class WorkWithMessages
    {
        public static void SendLeter(Message message)
        {
            Context context = new Context();
            context.Messages.Add(message);
            context.SaveChanges();
        }

        public static List<Message> GetMessages(string UserId)
        {
            Context context = new Context();
            return(context.Messages.Where(m => m.WhoGetUserId == UserId).OrderBy(m => m.Date).ToList());
        }
    }
}
