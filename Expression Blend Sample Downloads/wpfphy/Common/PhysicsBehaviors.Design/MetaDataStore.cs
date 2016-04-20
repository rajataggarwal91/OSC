using System;
using System.ComponentModel;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;

[assembly: ProvideMetadata(typeof(Spritehand.PhysicsBehaviors.Design.MetadataStore))]

namespace Spritehand.PhysicsBehaviors.Design
{

	/// <summary>
	/// Provides design-time metadata for all of the classes.
	/// </summary>
	public class MetadataStore : IProvideAttributeTable
	{
		#region IProvideAttributeTable Members

		public AttributeTable AttributeTable
		{
			get
			{
				AttributeTableBuilder attributeTableBuilder = new AttributeTableBuilder();

				System.Type type = typeof(Spritehand.PhysicsBehaviors.PhysicsObjectBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Provides physics for an element"));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsControllerBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Provides a physics controller for the scene to supply gravity"));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsMagnetBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Makes an element magnetic"));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsJointBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Allows an element to be grabbed by a point and dragged and will rotate if dragged off-center and rotation is enabled."));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsApplyForceBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Allows a force to be applied to an element"));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsApplyRotationBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Allows a rotational force to be applied to an element"));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsApplyTorqueBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Allows a torque force to be applied to an object"));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsCameraBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Allows a torque force to be applied to an object"));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsDestroyObjectBehavior);
				attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Allows a body object to be removed from a scene"));


				type = typeof(Spritehand.PhysicsBehaviors.PhysicsKeyTrigger);
                attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Take an action when a key is pressed."));

				type = typeof(Spritehand.PhysicsBehaviors.PhysicsCollisionTrigger);
                attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
				attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Raises an event when two bodies collide"));

                type = typeof(Spritehand.PhysicsBehaviors.PhysicsFluidContainerBehavior);
                attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
                attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Allows an object to behave like a container of fluid"));


#if SILVERLIGHT
                type = typeof(Spritehand.PhysicsBehaviors.PhysicsExplodeBehavior);
                attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
                attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Causes an element to explode"));

                type = typeof(Spritehand.PhysicsBehaviors.PhysicsSoundBehavior);
                attributeTableBuilder.AddCustomAttributes(type, new ToolboxCategoryAttribute("Physics"));
                attributeTableBuilder.AddCustomAttributes(type, new DescriptionAttribute("Plays mixed sound."));
#endif
                return attributeTableBuilder.CreateTable();
			}
		}

		/// <summary>
		/// Simple parser to provide descriptions and text to all of the type and properties.
		/// This just parses the XML intellisense file and pulls them from there. Quick and dirty :)
		/// </summary>
		/// <param name="assembly"></param>
		/// <param name="builder"></param>
		private void LoadComments(Assembly assembly, AttributeTableBuilder builder)
		{
			string xamlPath = new FileInfo(assembly.Location).Directory.FullName + @"\" + assembly.FullName.Split(',')[0] + ".xml";

			XDocument document = XDocument.Load(xamlPath);

			foreach (XElement member in document.Elements("doc").Elements("members").Elements("member"))
			{
				string name = member.Attribute("name").Value;

				string[] directives = name.Split(':');
				char commentType = directives[0][0];
				string summary = member.Element("summary").Value.Trim();

				switch (commentType)
				{
					case 'F':
						{
							string typeName = directives[1].Substring(0, directives[1].LastIndexOf('.'));
							string fieldName = directives[1].Substring(directives[1].LastIndexOf('.') + 1);
							Type type = assembly.GetType(typeName);
							if (type != null)
							{
								FieldInfo field = type.GetField(fieldName);
								if (field != null)
								{
									builder.AddCustomAttributes(type, fieldName, new DescriptionAttribute(summary));
								}
							}
						}
						break;
					case 'T':
						{
							Type type = assembly.GetType(directives[1]);
							if (type != null)
								builder.AddCustomAttributes(type, new DescriptionAttribute(summary));
						}
						break;
					case 'M':
						// Don't care about methods.
						break;
					case 'P':
						{
							string typeName = directives[1].Substring(0, directives[1].LastIndexOf('.'));
							string propertyName = directives[1].Substring(directives[1].LastIndexOf('.') + 1);
							Type type = assembly.GetType(typeName);
							if (type != null)
							{
								PropertyInfo field = type.GetProperty(propertyName);
								if (field != null)
								{
									builder.AddCustomAttributes(type, propertyName, new DescriptionAttribute(summary));
								}
							}
						}
						break;
					default:
						break;
				}

			}

		}

		#endregion
	}
}
