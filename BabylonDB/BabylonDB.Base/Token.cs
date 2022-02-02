namespace BabylonDB.Base;

public sealed class Token
{
	public TokenType Type { get; private set; }
	public string Lexeme { get; private set; }
	public int CurrentLine { get; private set; }

	public Token(TokenType type, string lexeme, int currentLine)
	{
		Type = type;
		Lexeme = lexeme;
		CurrentLine = currentLine;
	}
}