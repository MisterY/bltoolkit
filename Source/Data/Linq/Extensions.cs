﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace BLToolkit.Data.Linq
{
	using DataAccess;

	public static class Extensions
	{
		#region Table Helpers

		static public Table<T> GetTable<T>(this IDataContext dataContext)
			where T : class
		{
			return new Table<T>(dataContext);
		}

		static public Table<T> GetTable<T>(
			this IDataContext dataContext,
			object instance,
			[NotNull] MethodInfo methodInfo,
			[NotNull] params object[] parameters)
			where T : class
		{
			if (methodInfo == null) throw new ArgumentNullException("methodInfo");
			if (parameters == null) throw new ArgumentNullException("parameters");

			if (!typeof(Table<T>).IsAssignableFrom(methodInfo.ReturnType))
				throw new LinqException(
					"Method '{0}.{1}' must return type 'Table<{2}>'",
					methodInfo.Name, methodInfo.DeclaringType.FullName, typeof(T).FullName);

			Expression expr;

			if (parameters.Length > 0)
			{
				var pis  = methodInfo.GetParameters(); 
				var args = new List<Expression>(parameters.Length);

				for (var i = 0; i < parameters.Length; i++)
				{
					var type = pis[i].ParameterType;
					args.Add(Expression.Constant(parameters[i], type.IsByRef ? type.GetElementType() : type));
				}

				expr = Expression.Call(Expression.Constant(instance), methodInfo, args); 
			}
			else
				expr = Expression.Call(Expression.Constant(instance), methodInfo); 

			return new Table<T>(dataContext, expr);
		}

		#endregion

		#region Compile

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="dataContext"></param>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDc">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		static public Func<TDc,TResult> Compile<TDc,TResult>(
			[NotNull] this IDataContext dataContext,
			[NotNull] Expression<Func<TDc,TResult>> query)
			where TDc : IDataContext
		{
			return CompiledQuery.Compile(query);
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="dataContext"></param>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDc">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		static public Func<TDc,TArg1,TResult> Compile<TDc,TArg1, TResult>(
			[NotNull] this IDataContext dataContext,
			[NotNull] Expression<Func<TDc,TArg1,TResult>> query)
			where TDc : IDataContext
		{
			return CompiledQuery.Compile(query);
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="dataContext"></param>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDc">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg2">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		static public Func<TDc,TArg1,TArg2,TResult> Compile<TDc,TArg1,TArg2,TResult>(
			[NotNull] this IDataContext dataContext,
			[NotNull] Expression<Func<TDc,TArg1,TArg2,TResult>> query)
			where TDc : IDataContext
		{
			return CompiledQuery.Compile(query);
		}

		/// <summary>
		/// Compiles the query.
		/// </summary>
		/// <returns>
		/// A generic delegate that represents the compiled query.
		/// </returns>
		/// <param name="dataContext"></param>
		/// <param name="query">
		/// The query expression to be compiled.
		/// </param>
		/// <typeparam name="TDc">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg1">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg2">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TArg3">
		/// Represents the type of the parameter that has to be passed in when executing the delegate returned by the method.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// Returned type of the delegate returned by the method.
		/// </typeparam>
		static public Func<TDc,TArg1,TArg2,TArg3,TResult> Compile<TDc,TArg1,TArg2,TArg3,TResult>(
			[NotNull] this IDataContext dataContext,
			[NotNull] Expression<Func<TDc,TArg1,TArg2,TArg3,TResult>> query)
			where TDc : IDataContext
		{
			return CompiledQuery.Compile(query);
		}

		#endregion

		#region Object Operations

		#region Insert

		public static int Insert<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return Query<T>.Insert(dataContextInfo, obj);
		}

		public static int Insert<T>(this IDataContext dataContext, T obj)
		{
			return Query<T>.Insert(DataContextInfo.Create(dataContext), obj);
		}

		#endregion

		#region InsertOrUpdate

		[Obsolete("Use 'InsertOrReplace' instead.")]
		public static int InsertOrUpdate<T>(this IDataContextInfo dataContextInfo, T obj)
		{
			return InsertOrReplace(dataContextInfo, obj);
		}

		public static int InsertOrReplace<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return Query<T>.InsertOrReplace(dataContextInfo, obj);
		}

		[Obsolete("Use 'InsertOrReplace' instead.")]
		public static int InsertOrUpdate<T>(this IDataContext dataContext, T obj)
		{
			return InsertOrReplace(dataContext, obj);
		}

		public static int InsertOrReplace<T>(this IDataContext dataContext, T obj)
		{
			return Query<T>.InsertOrReplace(DataContextInfo.Create(dataContext), obj);
		}

		#endregion

		#region InsertBatch

#if !SILVERLIGHT

		public static int InsertBatch<T>(this DbManager dataContext, int maxBatchSize, IEnumerable<T> list)
		{
			return new SqlQuery<T>(dataContext).Insert(dataContext, maxBatchSize, list);
		}

		public static int InsertBatch<T>(this DbManager dataContext, IEnumerable<T> list)
		{
			return InsertBatch(dataContext, int.MaxValue, list);
		}

		public static int InsertBatch<T>(this DbManager dataContext, T[] list)
		{
			return InsertBatch(dataContext, int.MaxValue, list);
		}

		[Obsolete("Use InsertBatch instead.")]
		public static int Insert<T>(this DbManager dataContext, T[] list)
		{
			return Insert(dataContext, int.MaxValue, list);
		}

		[Obsolete("Use InsertBatch instead.")]
		public static int Insert<T>(this DbManager dataContext, int maxBatchSize, IEnumerable<T> list)
		{
			return new SqlQuery<T>().Insert(dataContext, maxBatchSize, list);
		}

		[Obsolete("Use InsertBatch instead.")]
		public static int Insert<T>(this DbManager dataContext, IEnumerable<T> list)
		{
			return Insert(dataContext, int.MaxValue, list);
		}

#endif

		#endregion

		#region InsertWithIdentity

		public static object InsertWithIdentity<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return Query<T>.InsertWithIdentity(dataContextInfo, obj);
		}

		public static object InsertWithIdentity<T>(this IDataContext dataContext, T obj)
		{
			return Query<T>.InsertWithIdentity(DataContextInfo.Create(dataContext), obj);
		}

		#endregion

		#region Update

		public static int Update<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return Query<T>.Update(dataContextInfo, obj);
		}

		public static int Update<T>(this IDataContext dataContext, T obj)
		{
			return Query<T>.Update(DataContextInfo.Create(dataContext), obj);
		}

#if !SILVERLIGHT

		public static int Update<T>(this DbManager dataContext, int maxBatchSize, IEnumerable<T> list)
		{
			return new SqlQuery<T>().Update(dataContext, maxBatchSize, list);
		}

		public static int Update<T>(this DbManager dataContext, IEnumerable<T> list)
		{
			return Update(dataContext, int.MaxValue, list);
		}

#endif

		#endregion

		#region Delete

		public static int Delete<T>([NotNull] this IDataContextInfo dataContextInfo, T obj)
		{
			if (dataContextInfo == null) throw new ArgumentNullException("dataContextInfo");
			return Query<T>.Delete(dataContextInfo, obj);
		}

		public static int Delete<T>([NotNull] this IDataContext dataContext, T obj)
		{
			return Query<T>.Delete(DataContextInfo.Create(dataContext), obj);
		}

#if !SILVERLIGHT

		public static int Delete<T>(this DbManager dataContext, int maxBatchSize, IEnumerable<T> list)
		{
			return new SqlQuery<T>().Delete(dataContext, maxBatchSize, list);
		}

		public static int Delete<T>(this DbManager dataContext, IEnumerable<T> list)
		{
			return Delete(dataContext, int.MaxValue, list);
		}

#endif

		#endregion

		#endregion
	}
}
