namespace ProjectCore
{
    public class Token
    {
        public int tag { get; }

        public Token(int tag)
        {
            this.tag = tag;
        }
    }

    public class Word : Token
    {
        public string Lexeme { get; }
        public Word(int tag, string lexeme) : base(tag)
        {
            this.Lexeme = lexeme;
        }
    }
}
