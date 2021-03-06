﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ModelGenerator.Core.Template
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\kunvu\source\repos\ModelGenerator\ModelGenerator.Core.Refined\Template\RepositoryBased_CSharp.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class RepositoryBased_CSharp : RepositoryBased_CSharpBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a ModelGenerator.
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using Utilities.Interfaces;
using Utilities.SQL.Extension;

namespace ");
            
            #line 24 "C:\Users\kunvu\source\repos\ModelGenerator\ModelGenerator.Core.Refined\Template\RepositoryBased_CSharp.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            
            #line default
            #line hidden
            this.Write(".Repositories.Based\r\n{\r\n    /// <summary>\r\n    /// Repository class designed for " +
                    "IDatabaseConnector.\r\n    /// </summary>\r\n    /// <typeparam name=\"T\"></typeparam" +
                    ">\r\n    /// <typeparam name=\"TDatabase\"></typeparam>\r\n    /// <typeparam name=\"TP" +
                    "arameter\"></typeparam>\r\n    public partial class Repository<T> : IEnumerable<T>\r" +
                    "\n        where T : class, new()\r\n    {\r\n        /// <summary>\r\n        /// Insta" +
                    "nce of database connector.\r\n        /// </summary>\r\n        protected readonly I" +
                    "DatabaseConnector Connector;\r\n        /// <summary>\r\n        /// Constructor.\r\n " +
                    "       /// </summary>\r\n        /// <param name=\"databaseConnector\">Instance of D" +
                    "atabaseConnector.</param>\r\n        public Repository(IDatabaseConnector database" +
                    "Connector)\r\n        {\r\n            Connector = databaseConnector;\r\n        }\r\n\r\n" +
                    "        /// <summary>\r\n        /// Delete data from repository.\r\n        /// </s" +
                    "ummary>\r\n        /// <param name=\"data\">Generic object.</param>\r\n        public " +
                    "virtual void Delete(T data)\r\n        {\r\n            Connector.Delete(data);\r\n   " +
                    "     }\r\n\r\n        /// <summary>\r\n        /// Delete data from repository.\r\n     " +
                    "   /// </summary>\r\n        /// <param name=\"key\">Primary key of target object.</" +
                    "param>\r\n        public virtual void Delete(object key)\r\n        {\r\n            C" +
                    "onnector.Delete<T>(key);\r\n        }\r\n\r\n        /// <summary>\r\n        /// Delete" +
                    " data from repository in an asynchronous manner.\r\n        /// </summary>\r\n      " +
                    "  /// <param name=\"data\">Generic object.</param>\r\n        public virtual async T" +
                    "ask DeleteAsync(T data)\r\n        {\r\n            await Connector.DeleteAsync(data" +
                    ").ConfigureAwait(false);\r\n        }\r\n\r\n        /// <summary>\r\n        /// Delete" +
                    " data from repository in an asynchronous manner.\r\n        /// </summary>\r\n      " +
                    "  /// <param name=\"key\">Primary key of target object.</param>\r\n        public vi" +
                    "rtual async Task DeleteAsync(object key)\r\n        {\r\n            await Connector" +
                    ".DeleteAsync<T>(key).ConfigureAwait(false);\r\n        }\r\n\r\n        /// <summary>\r" +
                    "\n        /// Insert data into repository.\r\n        /// </summary>\r\n        /// <" +
                    "param name=\"data\">Generic object.</param>\r\n        public virtual void Insert(T " +
                    "data)\r\n        {\r\n            Connector.Insert(data);\r\n        }\r\n\r\n        /// " +
                    "<summary>\r\n        /// Insert data into repository in an asynchronous manner.\r\n " +
                    "       /// </summary>\r\n        /// <param name=\"data\">Generic object.</param>\r\n " +
                    "       public virtual async Task InsertManyAsync(IEnumerable<T> data)\r\n        {" +
                    "\r\n            await Connector.InsertManyAsync(data).ConfigureAwait(false);\r\n    " +
                    "    }\r\n\r\n        /// <summary>\r\n        /// Insert data into repository.\r\n      " +
                    "  /// </summary>\r\n        /// <param name=\"data\">Generic object.</param>\r\n      " +
                    "  public virtual void InsertMany(IEnumerable<T> data)\r\n        {\r\n            Co" +
                    "nnector.InsertMany(data);\r\n        }\r\n\r\n        /// <summary>\r\n        /// Inser" +
                    "t data into repository in an asynchronous manner.\r\n        /// </summary>\r\n     " +
                    "   /// <param name=\"data\">Generic object.</param>\r\n        public virtual async " +
                    "Task InsertAsync(T data)\r\n        {\r\n            await Connector.InsertAsync(dat" +
                    "a).ConfigureAwait(false);\r\n        }\r\n\r\n        /// <summary>\r\n        /// Get a" +
                    "ll data from repository.\r\n        /// </summary>\r\n        /// <returns></returns" +
                    ">\r\n        public virtual IEnumerable<T> Query()\r\n        {\r\n            return " +
                    "Connector.Query<T>();\r\n        }\r\n\r\n        /// <summary>\r\n        /// Get data " +
                    "by specific condition from repository.\r\n        /// </summary>\r\n        /// <par" +
                    "am name=\"predicate\">Predicate condition.</param>\r\n        /// <returns></returns" +
                    ">\r\n        public virtual IEnumerable<T> Query(Expression<Func<T, bool>> predica" +
                    "te)\r\n        {\r\n            return Connector.Query<T>(predicate);\r\n        }\r\n\r\n" +
                    "        /// <summary>\r\n        /// Get data from repository.\r\n        /// </summ" +
                    "ary>\r\n        /// <param name=\"key\">Primary key of target object.</param>\r\n     " +
                    "   /// <returns></returns>\r\n        public virtual T Query(object key)\r\n        " +
                    "{\r\n            return Connector.Query<T>(key);\r\n        }\r\n\r\n        /// <summar" +
                    "y>\r\n        /// Get all data from repository in an asynchronous manner.\r\n       " +
                    " /// </summary>\r\n        /// <returns></returns>\r\n        public virtual async T" +
                    "ask<IEnumerable<T>> QueryAsync()\r\n        {\r\n            return await Connector." +
                    "QueryAsync<T>().ConfigureAwait(false);\r\n        }\r\n\r\n        /// <summary>\r\n    " +
                    "    /// Get data by specific condition from repository in an asynchronous manner" +
                    ".\r\n        /// </summary>\r\n        /// <param name=\"predicate\">Predicate conditi" +
                    "on.</param>\r\n        /// <returns></returns>\r\n        public virtual async Task<" +
                    "IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate)\r\n        {\r\n    " +
                    "        return await Connector.QueryAsync<T>(predicate).ConfigureAwait(false);\r\n" +
                    "        }\r\n\r\n        /// <summary>\r\n        /// Get data from repository.\r\n     " +
                    "   /// </summary>\r\n        /// <param name=\"key\">Primary key of target object.</" +
                    "param>\r\n        /// <returns></returns>\r\n        public virtual async Task<T> Qu" +
                    "eryAsync(object key)\r\n        {\r\n            return await Connector.QueryAsync<T" +
                    ">(key).ConfigureAwait(false);\r\n        }\r\n\r\n        /// <summary>\r\n        /// U" +
                    "pdate data in repository.\r\n        /// </summary>\r\n        /// <param name=\"data" +
                    "\">Generic object.</param>\r\n        public virtual void Update(T data)\r\n        {" +
                    "\r\n            Connector.Update(data);\r\n        }\r\n\r\n        /// <summary>\r\n     " +
                    "   /// Update data in repository in an asynchronous manner.\r\n        /// </summa" +
                    "ry>\r\n        /// <param name=\"data\">Generic object.</param>\r\n        public virt" +
                    "ual async Task UpdateAsync(T data)\r\n        {\r\n            await Connector.Updat" +
                    "eAsync(data).ConfigureAwait(false);\r\n        }\r\n        /// <summary>\r\n        /" +
                    "// Returns rows count from repository.\r\n        /// </summary>\r\n        /// <ret" +
                    "urns></returns>\r\n        public int Count()\r\n        {\r\n            return this." +
                    "Connector.Count<T>();\r\n        }\r\n\t\t/// <summary>\r\n        /// Returns rows coun" +
                    "t that is satisfied specific condition from repository.\r\n        /// </summary>\r" +
                    "\n        /// <returns></returns>\r\n        public int Count(Expression<Func<T,boo" +
                    "l>> predicate)\r\n        {\r\n            return this.Connector.Count<T>(predicate)" +
                    ";\r\n        }\r\n        /// <summary>\r\n        /// Filters a sequence of values ba" +
                    "sed on a predicate.\r\n        /// </summary>\r\n        /// <param name=\"predicate\"" +
                    "></param>\r\n        /// <returns></returns>\r\n        public IEnumerable<T> Where(" +
                    "Expression<Func<T, bool>> predicate)\r\n        {\r\n            return Query(predic" +
                    "ate);\r\n        }\r\n        /// <summary>\r\n        /// Returns a specified number " +
                    "of contiguous elements from the start of a sequence.\r\n        /// </summary>\r\n  " +
                    "      /// <param name=\"count\"></param>\r\n        /// <returns></returns>\r\n       " +
                    " public IEnumerable<T> Take(int count)\r\n        {\r\n            return Connector." +
                    "Query<T>(top: count);\r\n        }\r\n        /// <summary>\r\n        /// Get enumera" +
                    "tor of data repository.\r\n        /// </summary>\r\n        /// <returns></returns>" +
                    "\r\n        public IEnumerator<T> GetEnumerator()\r\n        {\r\n            foreach " +
                    "(var data in Query())\r\n            {\r\n                yield return data;\r\n      " +
                    "      }\r\n        }\r\n\r\n        IEnumerator IEnumerable.GetEnumerator()\r\n        {" +
                    "\r\n            return GetEnumerator();\r\n        }\r\n    }\r\n}\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 248 "C:\Users\kunvu\source\repos\ModelGenerator\ModelGenerator.Core.Refined\Template\RepositoryBased_CSharp.tt"

	public string Namespace {get; set;} = "YourNamespace";

        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class RepositoryBased_CSharpBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
