using System.Text.RegularExpressions;

namespace BabylonDB.Base;

public sealed class Tokenizer
{
	private readonly List<string> Lines;
	private bool EOF;

	private int lexemeLength;
	private int Line;
	private int Position;

	public Tokenizer(string source) =>
		Lines = new List<string>(Regex.Split(source, Environment.NewLine));

	private char GetChar()
	{
		if (EOF) return (char)0;
		var c = Lines[Line][Position];
		if (Position + 1 < Lines[Line].Length)
		{
			Position++;
		}
		else
		{
			if (Line + 1 < Lines.Count)
			{
				Line++;
				Position = 0;
			}
			else
			{
				EOF = true;
				Position++;
			}
		}
		return c;
	}

	private void UngetString(int count)
	{
		for (var i = 0; i < count; i++)
			UngetChar();
	}

	private void UngetChar()
	{
		if (Position != 0)
		{
			if (!EOF)
			{
				Position--;
			}
			else
			{
				Position--;
				EOF = false;
			}
		}
		else
		{
			Line--;
			Position = Lines[Line].Length - 1;
		}
	}

	private char PeekChar()
	{
		var c = GetChar();
		if (c != (char)0) UngetChar();
		return c;
	}

	public void Unget() => UngetString(lexemeLength);

	public Token Peek()
	{
		var token = Get();
		Unget();
		return token;
	}

	public Token? Get()
	{
		if (EOF) return null;
		TokenType type;
		var lexeme = string.Empty;
		if ((type = IsSpace()) != 0)
			return new Token(type, lexeme, Line);
		if ((type = IsOperator()) != 0)
			return new Token(type, lexeme, Line);
		if ((type = IsKeyword()) != 0)
			return new Token(type, lexeme, Line);
		var identifier = IsIdentifier();
		if (identifier.Item1 != 0)
			return new Token(TokenType.Identifier, identifier.Item2, Line);
		var integerLiteral = IsIntegerLiteral();
		if (integerLiteral.Item1 != 0)
			return new Token(TokenType.IntegerLiteral, integerLiteral.Item2, Line);
		//bad token
		return null;
	}

	private Tuple<TokenType, string> IsIntegerLiteral()
	{
		if (!char.IsDigit(PeekChar()))
			return new Tuple<TokenType, string>(0, string.Empty);
		var lexeme = GetChar().ToString();
		var count = 1;
		var line = Line;
		while (char.IsDigit(PeekChar()))
		{
			lexeme = lexeme + GetChar();
			count++;
			if (line != Line)
			{
				UngetString(count);
				return new Tuple<TokenType, string>(0, string.Empty);
			}
		}
		lexemeLength = count;
		return new Tuple<TokenType, string>(TokenType.Identifier, lexeme);
	}

	private TokenType IsKeyword()
	{
		if (!char.IsLetter(PeekChar())) return 0;
		var lexeme = GetChar().ToString();
		var count = 1;
		var line = Line;
		while (char.IsLetter(PeekChar()))
		{
			lexeme = lexeme + GetChar();
			count++;
			if (line != Line) break;
		}
		if (lexeme.ToUpper() == "Where")
		{
			lexemeLength = count;
			return TokenType.WhereKeyword;
		}
		else if (lexeme.ToUpper() == "VAR")
		{
			lexemeLength = count;
			return TokenType.VarKeyword;
		}
		else if (lexeme.ToUpper() == "Select")
		{
			lexemeLength = count;
			return TokenType.SelectKeyword;
		}
		else if (lexeme.ToUpper() == "First")
		{
			lexemeLength = count;
			return TokenType.FirstKeyword;
		}
		else if (lexeme.ToUpper() == "Between")
		{
			lexemeLength = count;
			return TokenType.BetweenKeyword;
		}
		else if (lexeme.ToUpper() == "Match")
		{
			lexemeLength = count;
			return TokenType.MatchKeyword;
		}
		UngetString(count);
		return 0;
	}

	private Tuple<TokenType, string> IsIdentifier()
	{
		if (!(char.IsLetter(PeekChar()) || PeekChar() == '_'))
			return new Tuple<TokenType, string>(0, string.Empty);
		var lexeme = GetChar().ToString();
		var count = 1;
		var line = Line;
		while (char.IsLetter(PeekChar()) || char.IsDigit(PeekChar()) || PeekChar() == '_')
		{
			lexeme = lexeme + GetChar();
			count++;
			if (line != Line)
			{
				UngetString(count);
				return new Tuple<TokenType, string>(0, string.Empty);
			}
		}
		lexemeLength = count;
		return new Tuple<TokenType, string>(TokenType.Identifier, lexeme);
	}

	private TokenType IsSpace()
	{
		if (char.IsWhiteSpace(PeekChar()))
		{
			GetChar();
			lexemeLength = 1;
			return TokenType.Space;
		}
		return 0;
	}

	private TokenType IsOperator()
	{
		var c = PeekChar();
		if (c == '=')
		{
			GetChar();
			if (PeekChar() == '=')
			{
				GetChar();
				lexemeLength = 2;
				return TokenType.EqualTo;
			}
			lexemeLength = 1;
			return TokenType.Assignment;
		}
		else if (c == '+')
		{
			GetChar();
			lexemeLength = 1;
			return TokenType.Plus;
		}
		else if (c == '-')
		{
			GetChar();
			lexemeLength = 1;
			return TokenType.Minus;
		}
		else if (c == '*')
		{
			GetChar();
			lexemeLength = 1;
			return TokenType.Multiplication;
		}
		else if (c == '/')
		{
			GetChar();
			lexemeLength = 1;
			return TokenType.Division;
		}
		else if (c == '%')
		{
			GetChar();
			lexemeLength = 1;
			return TokenType.Modulo;
		}
		else if (c == '!')
		{
			GetChar();
			if (PeekChar() == '=')
			{
				GetChar();
				lexemeLength = 2;
				return TokenType.NotEqualTo;
			}
			lexemeLength = 1;
			return TokenType.LogicalNOT;
		}
		else if (c == '>')
		{
			GetChar();
			if (PeekChar() == '=')
			{
				GetChar();
				lexemeLength = 2;
				return TokenType.GreaterThanOrEqualTo;
			}
			lexemeLength = 1;
			return TokenType.GeaterThan;
		}
		else if (c == '<')
		{
			GetChar();
			if (PeekChar() == '=')
			{
				GetChar();
				lexemeLength = 2;
				return TokenType.LessThanOrEqualTo;
			}
			lexemeLength = 1;
			return TokenType.LessThan;
		}
		else if (c == '&')
		{
			GetChar();
			if (PeekChar() == '&')
			{
				GetChar();
				lexemeLength = 2;
				return TokenType.LogicalAND;
			}
			lexemeLength = 1;
			return 0;
		}
		else if (c == '|')
		{
			GetChar();
			if (PeekChar() == '|')
			{
				GetChar();
				lexemeLength = 2;
				return TokenType.LogicalOR;
			}
			lexemeLength = 1;
			return 0;
		}
		else
		{
			return 0;
		}
	}
}