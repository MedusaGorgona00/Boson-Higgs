namespace BosonHiggsApi.BL.Models
{
    public class ErrorModel
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ErrorModel()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="messages"></param>
        public ErrorModel(List<string> messages, string? property = null)
        {
            Errors = new Dictionary<string, List<string>> { { property ?? string.Empty, messages } };
        }

        /// <summary>
        /// </summary>
        /// <param name="errors"></param>
        public ErrorModel(Dictionary<string, List<string>> errors)
        {
            Errors = errors;
        }

        /// <summary>
        ///     Error Dictionary
        /// </summary>
        public Dictionary<string, List<string>>? Errors { get; set; } = null;
    }
}
