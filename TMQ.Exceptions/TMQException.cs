using System.Net;

namespace TMQ.Exceptions
{
    public class TMQException : Exception
    {
        private TMQException()
        {
            Messages = new List<(string, string)>();
        }

        public TMQException(string? message) : this()
        {
            Messages.Add((string.Empty, message));
        }

        public TMQException(HttpStatusCode httpCode, string message) : this()
        {
            HttpCode = httpCode;
            Messages.Add((httpCode.ToString(), message));
        }

        public TMQException(HttpStatusCode httpCode) : this()
        {
            HttpCode = httpCode;
            Messages.Add((httpCode.ToString(), httpCode.ToString()));
        }

        public TMQException(IEnumerable<string>? messages) : this()
        {
            if (messages == null)
            {
                return;
            }

            foreach (var message in messages)
            {
                Add(message);
            }
        }

        public TMQException(HttpStatusCode httpCode, IEnumerable<string> messages) : this()
        {
            HttpCode = httpCode;
            foreach (var message in messages)
            {
                Add(message);
            }
        }

        public TMQException(string errorCode, string message) : this()
        {
            Messages.Add((errorCode, message));
        }

        public void Add(string message)
        {
            Messages.Add((string.Empty, message));
        }

        public void Add(string errorCode, string message)
        {
            Messages.Add((errorCode, message));
        }

        public List<(string, string)> Messages { get; set; }
        public HttpStatusCode HttpCode { get; set; }

        public new string Message
        {
            get
            {
                string message = string.Empty;
                foreach (var item in Messages)
                {
                    if (string.IsNullOrEmpty(item.Item1))
                    {
                        if (item.Item2?.Length > 0)
                        {
                            message += $"{item.Item2}, ";
                        }
                    }
                    else
                    {
                        if (item.Item2?.Length > 0)
                        {
                            message += $"{item.Item1}: {item.Item2}, ";
                        }
                        else
                        {
                            message += $"{item.Item1}, ";
                        }
                    }
                }

                return message.Trim().TrimEnd(',');
            }
        }
    }
}
