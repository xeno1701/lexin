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
EndOfFileToken,
NumberExpression,
BinaryExpression
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
        private int _position;

        public Lexer(string text)
        {
            _text = text;
        }

        private char currentChar {
            get {
                if (_position >= _text.Length) {
                    return '\0';
                }
                return _text[_position];
            }
        }

        private void Next() {
            _position++;
        }

        public SyntaxToken NextToken() {
            //Looking for numbers, operators and spaces.

            if (_position >= _text.Length) {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(currentChar)) {
                var start = _position;

                while (char.IsDigit(currentChar)) {
                    Next();
                }
                var length = _position - start;
                var text = _text.Substring(start, length);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(currentChar)) {
                var start = _position;

                while (char.IsWhiteSpace(currentChar)) {
                    Next();
                }
                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }
            switch (currentChar) {
                case '+':
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                 return new SyntaxToken(SyntaxKind.MultToken, _position++, "*", null);
                case '/':
                return new SyntaxToken(SyntaxKind.DivToken, _position++, "/", null);
                case '(':
                return new SyntaxToken(SyntaxKind.OpParenToken, _position++, "(", null);
                case ')':
                return new SyntaxToken(SyntaxKind.ClParenToken, _position++, ")", null);
            }
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        
        
        
        
        }   


    }

abstract class SyntaxNode {
    public abstract SyntaxKind Kind {get;}
}

abstract class ExpressionSyntax : SyntaxNode {

}

sealed class NumberExpressionSyntax : SyntaxNode {
    public NumberExpressionSyntax(SyntaxToken numberToken)
    {
        NumberToken = numberToken;
    }
    public SyntaxToken NumberToken {get;}
    public override SyntaxKind Kind => SyntaxKind.NumberExpression;
}

sealed class BinaryExpressionSyntax : ExpressionSyntax {
    public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxNode operatorToken, ExpressionSyntax right)
    {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
    }
    public ExpressionSyntax Left {get;}
    public SyntaxNode OperatorToken {get;}
    public ExpressionSyntax Right {get;}
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
}

class Parser {
    private readonly SyntaxToken[] _tokens;
    private int _position;

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
            var index = _position + offset;
            if (index >= _tokens.Length) {
                return _tokens[_tokens.Length - 1];
            }
            return _tokens[index];
        }

    private SyntaxToken current => Peek(0);
    
    }

}

    



