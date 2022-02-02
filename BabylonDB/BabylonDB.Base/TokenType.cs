namespace BabylonDB.Base;

public enum TokenType : ushort
{

	// keyword
	WhereKeyword = 1, // Where
	VarKeyword = 2, // VAR
	SelectKeyword = 3, // Select
	FirstKeyword = 4, // First
	BetweenKeyword = 5, // Between
	MatchKeyword = 6, // Match
	OrderKeyword =7,  // Order

	// literal
	IntegerLiteral = 10,

	// identifier
	Identifier = 20,

	// operator
	Assignment = 30, // =
	Plus = 31, // +
	Minus = 32, // -
	Multiplication = 33, // *
	Division = 34, // /
	Modulo = 35, // %

	EqualTo = 36, // ==
	NotEqualTo = 37, // !=
	GeaterThan = 38, // >
	LessThan = 39, // <
	GreaterThanOrEqualTo = 40, // >=
	LessThanOrEqualTo = 41, // <=

	// logical operators
	LogicalNOT = 42, // !
	LogicalAND = 43, // &&
	LogicalOR = 44, // ||

	// trivia
	Space = 60,
}