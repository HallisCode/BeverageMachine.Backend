using System;

namespace REST.AccessRules
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class QueryKeyAccessAttribute : Attribute
	{
	}
}
