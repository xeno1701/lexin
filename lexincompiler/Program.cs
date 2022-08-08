using System;
using System.Collections.Generic;

namespace lexincompiler  {
class Program {
        static void Main(string[] args) {
            while (true) {
                Console.Write("$> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) {
                    return;
                }
                var lexer = new Lexer(line);
                while (true) {
                    var token = lexer.NextToken(); 
                    if (token.Kind == SyntaxKind.EndOfFileToken) {
                        break;
                    }
                    Console.Write($"{token.Kind}: '{token.Text}' ");
                    if (token.Value != null) {
                        Console.Write($"{token.Value}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
enum SyntaxKind {
NumberToken,
WhitespaceToken,
PlusToken,
MinusToken,
MultToken,
DivToken,
OpParenToken,
ClParenToken,
BadToken,
EndOfFileToken
    }
class SyntaxToken {
    public SyntaxToken(SyntaxKind kind, int position, string text, object value)
    {
        Kind = kind;
        Position = position;
        Text = text;
        Value = value;
    }

    public SyntaxKind Kind {get;}
    public int Position {get;}
    public string Text {get;}
    public object Value {get;}
}
    class Lexer {
        
        private readonly string _text;
        private int _postion;

        public Lexer(string text)
        {
            _text = text;
        }

        private char currentChar {
            get {
                if (_postion >= _text.Length) {
                    return '\0';
                }
                return _text[_postion];
            }
        }

        private void Next() {
            _postion++;
        }

        public SyntaxToken NextToken() {
            //Looking for numbers, operators and spaces.

            if (_postion >= _text.Length) {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _postion, "\0", null);
            }

            if (char.IsDigit(currentChar)) {
                var start = _postion;

                while (char.IsDigit(currentChar)) {
                    Next();
                }
                var length = _postion - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(currentChar)) {
                var start = _postion;

                while (char.IsWhiteSpace(currentChar)) {
                    Next();
                }
                var length = _postion - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            if (currentChar == '+') {
                return new SyntaxToken(SyntaxKind.PlusToken, _postion++, "+", null);
            }if (currentChar == '-') {
                return new SyntaxToken(SyntaxKind.MinusToken, _postion++, "-", null);
            }if (currentChar == '*') {
                return new SyntaxToken(SyntaxKind.MultToken, _postion++, "*", null);
            }if (currentChar == '/') {
                return new SyntaxToken(SyntaxKind.DivToken, _postion++, "/", null);
            }if (currentChar == '(') {
                return new SyntaxToken(SyntaxKind.OpParenToken, _postion++, "(", null);
            }if (currentChar == ')') {
                return new SyntaxToken(SyntaxKind.ClParenToken, _postion++, ")", null);
            }
            return new SyntaxToken(SyntaxKind.BadToken, _postion++, _text.Substring(_postion - 1, 1), null);
        
        
        
        
        }   


    }

class SyntaxNode {
    public abst
}

class Parser {
    private readonly SyntaxToken[] _tokens;
    private int _postion;

    public Parser(string text)
    {
        var tokens = new List<SyntaxToken>();
        var lexer = new Lexer(text);
        SyntaxToken token;
        do {
            token = lexer.NextToken();
            if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken) {
                tokens.Add(token);
            }
        } while (token.Kind != SyntaxKind.EndOfFileToken);
        
        _tokens = tokens.ToArray();
    }
        private SyntaxToken Peek(int offset) {
            var index = _postion + offset;
            if (index >= _tokens.Length) {
                return _tokens[_tokens.Length - 1];
            }
            return _tokens[index];
        }
        
    private SyntaxToken current => Peek(0);
    
    }

}

    



