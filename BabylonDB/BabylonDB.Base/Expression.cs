using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabylonDB.Base;

public abstract class Expression
{
	public Expression Right;
	public  Tokenizer OperatorSign;
	public Expression Left;

	protected Expression(Expression right, Tokenizer operatorSign, Expression left)
	{
		Right = right;
		OperatorSign = operatorSign;
		Left = left;
	}
}