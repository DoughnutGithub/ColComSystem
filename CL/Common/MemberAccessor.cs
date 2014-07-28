using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CL.Interfaces;

namespace CL.Common
{

    /// <summary>
    /// 属性访问器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="P"></typeparam>
    internal class PropertyAccessor<T, P> : INamedMemberAccessor
    {
        private Func<T, P> m_GetValueDelegate;
        private Action<T, P> m_SetValueDelegate;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {

            var getMethodInfo = propertyInfo.GetGetMethod();
            if (null != getMethodInfo)
            {
                m_GetValueDelegate = (Func<T, P>)Delegate.CreateDelegate(typeof(Func<T, P>), getMethodInfo);
            }

            var setMethodInfo = propertyInfo.GetSetMethod();
            if (null != setMethodInfo)
            {
                m_SetValueDelegate = (Action<T, P>)Delegate.CreateDelegate(typeof(Action<T, P>), setMethodInfo);
            }
        }

        public object GetValue(object instance)
        {
            return m_GetValueDelegate((T)instance);
        }

        public void SetValue(object instance, object newValue)
        {
            m_SetValueDelegate((T)instance, (P)newValue);
        }
    }

    /// <summary>  
    /// 
    /// </summary>  
    public class MemberAccessor<T> : IMemberAccessor
    {
        #region Singleton
        private MemberAccessor()
        {
            Type type = typeof(T);
            foreach (PropertyInfo propInfo in StaticCache<Type, PropertyInfo[]>.Get(type, p => p.GetProperties()))
            {
                INamedMemberAccessor accessor = Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(type, propInfo.PropertyType), propInfo) as INamedMemberAccessor;
                m_accessorCache.Add(type.FullName + propInfo.Name, accessor);
            }
        }
        public static MemberAccessor<T> Instance
        {
            get { return Nested.m_instance; }
        }
        private class Nested
        {
            static Nested() { }
            internal static readonly MemberAccessor<T> m_instance = new MemberAccessor<T>();
        }
        #endregion

        private static Dictionary<string, INamedMemberAccessor> m_accessorCache = new Dictionary<string, INamedMemberAccessor>();

        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="instance"></param>  
        /// <param name="memberName"></param>  
        /// <returns>The member value</returns>  
        public object GetValue(object instance, string memberName)
        {
            INamedMemberAccessor ma = FindAccessor(instance, memberName);
            return ma.GetValue(instance);
        }

        /// <summary>  
        /// 
        /// </summary>  
        /// <param name="instance"></param>  
        /// <param name="memberName"></param>  
        /// <param name="newValue"></param>  
        public void SetValue(object instance, string memberName, object newValue)
        {
            INamedMemberAccessor ma = FindAccessor(instance, memberName);
            ma.SetValue(instance, newValue);
        }

        private INamedMemberAccessor FindAccessor(object instance, string memberName)
        {
            Type type = typeof(T);
            string key = type.FullName + memberName;
            return m_accessorCache[key];
        }
    }
}
