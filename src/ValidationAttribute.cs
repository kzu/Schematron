using System;
using System.Web.Services.Protocols;

namespace NMatrix.Schematron
{
	/// <summary>
	/// Attribute applied to webmethods that require Schematron validation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class ValidationAttribute : SoapExtensionAttribute
	{
		#region Ctor Overloads

		/// <summary>
		/// Initializes the attribute for a certain schema.
		/// </summary>
		/// <param name="schema">The schema to validate the webmethod with.</param>
		public ValidationAttribute(string schema) : this(new string[] { schema }, OutputFormatting.Simple)
		{
		}

		/// <summary>
		/// Initializes the attribute for a certain schema.
		/// </summary>
		/// <param name="schemas">The set of schemas to validate the webmethod with.</param>
		public ValidationAttribute(string[] schemas) : this(schemas, OutputFormatting.Simple)
		{
		}

		/// <summary>
		/// Initializes the attribute for a certain schema.
		/// </summary>
		/// <param name="schema">The schema to validate the webmethod with.</param>
		/// <param name="formatting">The formatting to use for the validation output.</param>
		public ValidationAttribute(string schema, OutputFormatting formatting) : this(new string[] { schema }, formatting)
		{
		}

		/// <summary>
		/// Initializes the attribute for a certain schema.
		/// </summary>
		/// <param name="schemas">The set of schemas to validate the webmethod with.</param>
		/// <param name="formatting">The formatting to use for the validation output.</param>
		public ValidationAttribute(string[] schemas, OutputFormatting formatting) 
		{
			this.schemas = schemas;
			this.formatting = formatting;
		}

		#endregion Ctor Overloads

		#region Properties

		/// <summary>
		/// Gets the location of the schemas to use.
		/// </summary>
		public string[] Schemas
		{
			get { return schemas; }
			set { schemas = value; }
		} string[] schemas;

		/// <summary>
		/// Gets/Sets the phase to use for validation.
		/// </summary>
		public string Phase
		{
			get { return phase; }
			set { phase = value; }
		} string phase;

		/// <summary>
		/// Gets/Sets the formatting to use for validation errors.
		/// </summary>
		public OutputFormatting Formatting
		{
			get { return formatting; }
			set { formatting = value; }
		} OutputFormatting formatting;

		/// <summary>
		/// Gets/Sets a type that implements <see cref="Formatters.IFormatter"/> to use 
		/// to format error messages. If set, this property takes precedence 
		/// over <see cref="Formatting"/>.
		/// </summary>
		public Type FormattingType
		{
			get { return formatType; }
			set { formatType = value; }
		} Type formatType;

		#endregion Properties

		#region Overrides
		
		/// <summary>
		/// Gets the type of this extension attribute, which is always 
		/// <see cref="ValidationExtension"/>.
		/// </summary>
		public override Type ExtensionType
		{
			get { return typeof(ValidationExtension); }
		}

		/// <summary>
		/// Gets/Sets the priority of the extension.
		/// </summary>
		public override int Priority
		{
			get { return priority; }
			set { priority = value; }
		} int priority = 0;

		#endregion Overrides
	}
}
