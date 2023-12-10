using System.ComponentModel.DataAnnotations;

namespace BosonHiggsApi.BL.Models
{
    public static class MessageModel
    {
        public class In
        {
            public string UserToken { get; set; }

            [MaxLength(256)]
            public string Text { get; set; }
        }

        public class Out
        {
            public int Id { get; set; }
            public string NickName { get; set; }
            public string Text { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime CreatedDateTime { get; set; }
        }
    }
}
