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

                    var lenght = start - _position;
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
                    int.TryParse(text, out var value);
                    return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, value);
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

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position -1, 1), null);
        }
    }
}