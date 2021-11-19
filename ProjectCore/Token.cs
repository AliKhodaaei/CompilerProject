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

    public class Num : Token
    {
        public int value { get; }
        public Num(int value) : base(Tag.num)
        {
            this.value = value;
        }
    }

    public class Word : Token
    {
        public string lexeme { get; }
        public Word(int tag, string lexeme) : base(tag)
        {
            this.lexeme = lexeme;
        }
    }
}
