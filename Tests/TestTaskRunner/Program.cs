using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using FluentConfigurationTests;
using NUnit.Framework;

using ArtOfNet.Framework.Core.Test;

namespace TestTaskRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestTaskHelper.RunAllFor<DynamicXmlTests>();
            Console.Read();
        }

        
        

    }

    //public class NotTestableException: Exception
    //{
    //    public NotTestableException(){}
    //    public NotTestableException(string message):base(message){}
    //}

    //public class TestTask<T> where T: class, new()
    //{
    //    private T _cachedTaskFixture;
    //    private List<MethodInfo> _testMethods;

    //    public TestTask()
    //    {
    //        //verify that class is really a test fixture
    //        TestTaskHelper.EnsureTypeIsFixture<T>();
    //        //then create an instance if ok
    //        _cachedTaskFixture = Activator.CreateInstance<T>();
    //        //if instance is null, it must be an error, throw it
    //        if (_cachedTaskFixture == null)
    //        {
    //            throw new ArgumentNullException("T", "can not create an instance of T");
    //        }
    //        //then load in cache a list of available test methods
    //        LoadTestMethods();
    //    }

    //    public bool Test(Expression<Action<T>> selectedTest)
    //    {
    //        if (_cachedTaskFixture != null)
    //        {
    //            MethodCallExpression bodyMethod = (selectedTest.Body) as MethodCallExpression;
    //            if (bodyMethod == null)
    //            {
    //                throw new NotTestableException("The method you try to call is not valid");
    //            }
    //            var findMethod = from test in _testMethods
    //                             where test == bodyMethod.Method
    //                             select test;
    //            if (findMethod == null)
    //            {
    //                throw new NotTestableException("The method you try to call is not a test");
    //            }
    //            if (findMethod.Count() == 1)
    //            {
    //                Action<T> action = selectedTest.Compile();
    //                action.DynamicInvoke(_cachedTaskFixture);
    //            }
    //            else
    //            {
    //                throw new NotTestableException("The method you try to call is not valid"); ;
    //            }
    //            return true;
    //        }
    //        return false;
    //    }

    //    public bool RunAll(Action<string> beforeStart, Action<string> successRun, Action<string> failedRun)
    //    {
    //        bool result = true;
    //        foreach (MethodInfo method in _testMethods)
    //        {
    //            beforeStart(method.ToString());
    //            try
    //            {
    //                method.Invoke(_cachedTaskFixture, null);
    //                successRun("Result   : ok");
    //                result &= true;
    //            }
    //            catch (Exception e)
    //            {
    //                failedRun(string.Format("Result  : {0}", e.Message));
    //                result &= false;
    //            }
    //        }
    //        return result;
    //    }
    //    private void LoadTestMethods()
    //    {
    //        Type type = typeof(T);

    //        var methods = type.GetMethods();

    //        if (methods == null)
    //        {
    //            return;
    //        }

    //        var testMethods = from m in methods
    //                          let customAttrib = m.GetCustomAttributes(typeof(TestAttribute), true)
    //                          where customAttrib != null && customAttrib.Count() > 0
    //                          select m;
    //        _testMethods = testMethods.ToList();
    //    }

        
    //}

    //public static class TestTaskHelper
    //{
    //    public static bool IsTestFixture(Type t)
    //    {
    //        var attributes = t.GetCustomAttributes(true);
    //        if (attributes == null)
    //        {
    //            return false;
    //        }

    //        var isFixtureQuery = from a in attributes
    //                             where a.ToString().Contains("TestFixture")
    //                             select a;
    //        if (isFixtureQuery.Count() > 0)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    public static ConsoleTestMessageHelper Message = new ConsoleTestMessageHelper();
    //    public static void EnsureTypeIsFixture<T>()
    //    {
    //        if (!IsTestFixture(typeof(T)))
    //        {
    //            throw new NotTestableException(string.Format("Type [{0}] is not a TestFixture", typeof(T).Name));
    //        }
    //    }

    //    public static void Test<T>(Expression<Action<T>> selectedTest) where T : class, new()
    //    {
    //        string typeSource = (selectedTest.Type.GetGenericArguments()[0]).FullName;
    //        string methodName = selectedTest.ToString();
    //        Message.WriteLineNormal("====================================================");
    //        Message.WriteLineNormal("Type   : {0}", typeSource);
    //        Message.WriteLineNormal("Test   : {0}", methodName);
    //        Message.WriteLineNormal("====================================================");
            
    //        try
    //        {
    //            var task = new TestTask<T>();
    //            task.Test(selectedTest);
    //            Message.WriteLineSuccess("Result   : ok");
    //        }
    //        catch (Exception e)
    //        {
    //            Message.WriteLineError("Result  : {0}",e.Message);
    //        }
    //        Message.WriteLineNormal("====================================================");
            
    //        Message.WriteLineNormal("***");
    //    }

    //    public static void RunAllFor<T>() where T : class, new()
    //    {
    //        string typeSource = typeof(T).FullName;
    //        Message.WriteLineNormal("====================================================");
    //        Message.WriteLineNormal("Type   : {0}", typeSource);
    //        Message.WriteLineNormal("====================================================");

    //        var task = new TestTask<T>();
    //        task.RunAll(
    //            (method) => Message.WriteLineNormal("- Test   : {0}", method),
    //            (success) => Message.WriteLineSuccess(" - Result   : ok"),
    //            (fail) => Message.WriteLineError("  - Result  : {0}", fail)
    //            );

    //        Message.WriteLineNormal("====================================================");
    //        Message.WriteLineNormal("***");
    //    }
    //}

    //public class ConsoleTestMessageHelper
    //{
    //    public void WriteNormal(string message, params object[] values)
    //    {
    //        WriteHelper(ConsoleColor.White, false, message, values);
    //    }
    //    public void WriteLineNormal(string message, params object[] values)
    //    {
    //        WriteHelper(ConsoleColor.White, true, message, values);
    //    }
    //    public void WriteSuccess(string message, params object[] values)
    //    {
    //        WriteHelper(ConsoleColor.Green, false, message, values);
    //    }
    //    public void WriteLineSuccess(string message, params object[] values)
    //    {
    //        WriteHelper(ConsoleColor.Green, true, message, values);
    //    }
    //    public void WriteError(string message, params object[] values)
    //    {
    //        WriteHelper(ConsoleColor.Red, false, message, values);
    //    }
    //    public void WriteLineError(string message, params object[] values)
    //    {
    //        WriteHelper(ConsoleColor.Red, true, message, values);
    //    }

    //    public void WriteHelper(ConsoleColor color, bool newline, string message, params object[] values)
    //    {
    //        ConsoleColor current = Console.ForegroundColor;
    //        Console.ForegroundColor = color;
    //        if (newline)
    //        {
    //            Console.WriteLine(message, values);
    //        }
    //        else
    //        {
    //            Console.Write(message, values);
    //        }

    //        Console.ForegroundColor = current;
    //    }

    //}
}
