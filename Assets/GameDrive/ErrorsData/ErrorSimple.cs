using System;

namespace GameDrive
{
    [Serializable]
    public class ErrorSimple
    {
        public string code;
        public string message;
        public ErrorSimple()
        {
        }
        public ErrorSimple(string code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public override string ToString()
        {
            return "code:" + this.code + ", message:" + this.message;
        }

        public Exception ToException()
        {
            return new Exception(this.ToString());
        }
    }
}