using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Globalization;

namespace MirkW
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write(" > ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    return;
                }

                var lexer = new Lexer(line);

                while(true) {
                    var token = lexer.NextToken();

                    if (token.Kind == SyntaxKind.EndOfFileToken)
                        break;
                    Console.Write($"{token.Kind}: {token.Text}");
                    if(token.Value is not null)
                    {
                        Console.WriteLine($"{token.Value}");
                    }
                    Console.WriteLine();
                    
                }
            }
        }
    }
    //Lists the kinds that we have.
    enum SyntaxKind
    {
        NumberToken, 
        WhiteSpaceToken, 
        PlusToken, 
        MinusToken, 
        MultiplieToken, 
        DivideToken, 
        OpenParentesisToken, 
        CloseParentesisToken,
        BadToken,
        EndOfFileToken
    }

    class SyntaxToken
    {
        
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;

        }
        //Verifies the kind of the variable
        public SyntaxKind Kind;
        public int Position;
        public string Text;
        public object Value;
    }

    //This represent the Lexer.
    class Lexer
    {
        private readonly string _text;
        private int _position;

        public Lexer(string text)
        {
            _text = text;
        }

        public char Current
        {
            get
            {
                //Verifies if the position informed is outside the bonds of the text. If it is, then it finishes the execution
                if (_position >= _text.Length)
                    return '\0';

                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken NextToken()
        {
            if  (char.IsDigit(Current))
            {
                var start = _position;

                while (char.IsDigit(Current))
                {
                    Next();

                    var lenght = _position - start;
                    var text = _text.Substring(start, lenght);
                    int.TryParse(text, out var value);
                    return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
                }
            }

            if (char.IsWhiteSpace(Current))
            {
                var start = _position;

                while (char.IsDigit(Current))
                {
                    Next();

                    var lenght = start - _position;
                    var text = _text.Substring(start, lenght);
                    return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, null);
                }
            }

            if (Current == '+')
            {
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            }
            else if(Current == '-')
            {
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            }
            else if (Current == '*')
            {
                return new SyntaxToken(SyntaxKind.MultiplieToken, _position++, "*", null);
            }
            else if (Current == '/')
            {
                return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
            }
            else if (Current == '(')
            {
                return new SyntaxToken(SyntaxKind.OpenParentesisToken, _position++, "(", null);
            }
            else if (Current == ')')
            {
                return new SyntaxToken(SyntaxKind.CloseParentesisToken, _position++, ")", null);
            }

            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, string.Empty, null);
            }

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }

    class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;

        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();
            var lexer = new Lexer(text);

            SyntaxToken token;

            do
            {
                token = lexer.NextToken();
                if (token.Kind != SyntaxKind.BadToken && token.Kind != SyntaxKind.WhiteSpaceToken)
                {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken); 

            _tokens = tokens.ToArray();
        }

        public SyntaxToken Peek(int offset)
        {
            var index = _position + offset;

            if (index >= _tokens.Length)
            {
                return _tokens[_position - 1];
            }
            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);
    }
}