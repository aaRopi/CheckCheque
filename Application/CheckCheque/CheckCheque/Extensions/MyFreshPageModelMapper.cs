using System;
using System.Collections.Generic;
using FreshMvvm;

namespace CheckCheque.Extensions
{
    public class MyFreshPageModelMapper : IFreshPageModelMapper
    {
        public static IDictionary<Type, Type> Mappings = new Dictionary<Type, Type>();

        public string GetPageTypeName(Type pageModelType)
        {
            return Mappings.ContainsKey(pageModelType)
                ? Mappings[pageModelType].AssemblyQualifiedName
                : throw new Exception("View model has not been registered!");
        }
    }
}
