/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  URL: http://www.codeproject.com/Articles/19911/Dynamically-Invoke-A-Method-Given-Strings-with-Met
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  
 * Usage
 * GenerateOutput is a method in TestClass
 * TypeUtils.InvokeStringMethod("TestClass", "GenerateOutput");
 * 
 * AppendString is a method in TestClass2. AppendString takes one parameter, which has here been set to "AppendToMe!".
 * TypeUtils.InvokeStringMethod2("TestClass2", "AppendString", "AppendToMe!");
 * 
 * OutputMethod is a method in TestClass3. TestClass3 in turn lives in project TestProject, and has namespace TestNamespace.
 * TypeUtils.InvokeStringMethod3("TestProject","TestNamespace","TestClass3","OutputMethod");
 * 
 */

using System;
using System.Linq;
using System.Reflection;

namespace BLL
{

    public class TypeUtils
    {

        /// ///////////////////// InvokeStringMethod //////////////////////
        ///
        /// <summary>
        /// Calls a static public method. 
        /// Assumes that the method returns a string, and doesn't have parameters.
        /// </summary>
        /// <param name="typeName">name of the class in which the method lives.</param>
        /// <param name="methodName">name of the method itself.</param>
        /// <returns>the string returned by the called method.</returns>
        /// 
        public static string InvokeStringMethod(string typeName, string methodName)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(typeName);

            // Invoke the method itself. The string returned by the method winds up in s
            String s = (String)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                            null,
                            null,
                            null);

            // Return the string that was returned by the called method.
            return s;
        }

        /// ///////////////////// InvokeStringMethod2 //////////////////////
        ///
        /// <summary>
        /// Calls a static public method. 
        /// Assumes that the method returns a string, and takes a string parameter.
        /// </summary>
        /// <param name="typeName">name of the class in which the method lives.</param>
        /// <param name="methodName">name of the method itself.</param>
        /// <param name="stringParam">parameter passed to the method.</param>
        /// <returns>the string returned by the called method.</returns>
        /// 
        public static string InvokeStringMethod2(string typeName, string methodName, string stringParam)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(typeName);

            // Invoke the method itself. The string returned by the method winds up in s.
            // Note that stringParam is passed via the last parameter of InvokeMember,
            // as an array of Objects.
            String s = (String)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                            null,
                            null,
                            new Object[] { stringParam });

            // Return the string that was returned by the called method.
            return s;
        }

        /// ///////////////////// InvokeStringMethod3 //////////////////////
        ///
        /// <summary>
        /// Calls a static public method. 
        /// Assumes that the method returns a string, and doesn't have parameters.
        /// </summary>
        /// <param name="assemblyName">name of the assembly containing the class in which the method lives.</param>
        /// <param name="namespaceName">namespace of the class.</param>
        /// <param name="typeName">name of the class in which the method lives.</param>
        /// <param name="methodName">name of the method itself.</param>
        /// <returns>the string returned by the called method.</returns>
        /// 
        public static string InvokeStringMethod3(string assemblyName, string namespaceName, string typeName, string methodName)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(String.Format("{0}.{1},{2}", namespaceName, typeName, assemblyName));

            // Invoke the method itself. The string returned by the method winds up in s
            String s = (String)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                            null,
                            null,
                            null);

            // Return the string that was returned by the called method.
            return s;
        }

        #region AddedSection
        //Added functions by Matthew David Elgert - mdelgert@yahoo.com
        /// ///////////////////// InvokeStringMethod4 //////////////////////
        ///
        /// <summary>
        /// Calls a static public method. 
        /// Assumes that the method returns a string, and doesn't have parameters.
        /// </summary>
        /// <param name="assemblyName">name of the assembly containing the class in which the method lives.</param>
        /// <param name="namespaceName">namespace of the class.</param>
        /// <param name="typeName">name of the class in which the method lives.</param>
        /// <param name="methodName">name of the method itself.</param>
        /// /// <param name="stringParam">parameter passed to the method.</param>
        /// <returns>the string returned by the called method.</returns>
        /// 
        public static string InvokeStringMethod4(string assemblyName, string namespaceName, string typeName, string methodName, string stringParam)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(String.Format("{0}.{1},{2}", namespaceName, typeName, assemblyName));

            // Invoke the method itself. The string returned by the method winds up in s.
            // Note that stringParam is passed via the last parameter of InvokeMember, as an array of Objects.
            String s = (String)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                            null,
                            null,
                            new Object[] { stringParam });

            // Return the string that was returned by the called method.
            return s;
        }

        /// ///////////////////// InvokeStringMethod5 //////////////////////
        ///
        /// <summary>
        /// Calls a static public method. 
        /// Assumes that the method returns a string
        /// </summary>
        /// <param name="assemblyName">name of the assembly containing the class in which the method lives.</param>
        /// <param name="namespaceName">namespace of the class.</param>
        /// <param name="typeName">name of the class in which the method lives.</param>
        /// <param name="methodName">name of the method itself.</param>
        /// <param name="stringParam">parameter passed to the method.</param>
        /// <returns>the string returned by the called method.</returns>
        /// 
        public static string InvokeStringMethod5(string assemblyName, string namespaceName, string typeName, string methodName, string stringParam)
        {
            //This method was created incase Method has params with default values. If has params will return error and not find function
            //This method will choice and is the preffered method for invoking 
            
            // Get the Type for the class
            Type calledType = Type.GetType(String.Format("{0}.{1},{2}", namespaceName, typeName, assemblyName));
            String s = null;

            // Invoke the method itself. The string returned by the method winds up in s.
            // Note that stringParam is passed via the last parameter of InvokeMember, as an array of Objects.

            if (MethodHasParams(assemblyName, namespaceName, typeName, methodName))
            {
                s = (String)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                            null,
                            null,
                            new Object[] { stringParam });
            }
            else
            {
                s = (String)calledType.InvokeMember(
                methodName,
                BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                null,
                null,
                null);
            }
            
            // Return the string that was returned by the called method.
            return s;

        }

        public static bool MethodHasParams(string assemblyName, string namespaceName, string typeName, string methodName)
        {
            bool HasParams;
            string name = String.Format("{0}.{1},{2}", namespaceName, typeName, assemblyName);
            Type calledType = Type.GetType(name);
            MethodInfo methodInfo = calledType.GetMethod(methodName);
            ParameterInfo[] parameters = methodInfo.GetParameters();
            
            if (parameters.Length > 0)
            {
                HasParams = true;
            }
            else
            {
                HasParams = false;
            }

            return HasParams;

        }

        #endregion

    }

}
