using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabylonDB.Base;

public class BinaryExpression : Expression
{
	public BinaryExpression(Expression left, Tokenizer operatorSign, Expression right) : base(left, operatorSign, right)
	{

	}
}