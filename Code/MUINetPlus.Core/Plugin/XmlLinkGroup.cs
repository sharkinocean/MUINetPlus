﻿using System;
using System.Xml;

namespace MUINetPlus.Core
{
    public class XmlLinkGroup
    {
        public void Setup(XmlReader reader, ref ExtensionData extension)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals("Extension"))
                {
                    break;
                }
                if (reader.NodeType == XmlNodeType.Element && reader.IsStartElement() && reader.LocalName.Equals("LinkGroup"))
                {
                    var existValue = ExtensionCheck.LinkGroupExistValue<LinkGroupData>
                        (
                            extension,
                            node =>
                            {
                                var n = node as LinkGroupData;
                                if (n != null)
                                {
                                    var name = reader.GetAttribute("Name");
                                    var groupKey = reader.GetAttribute("GroupKey") ?? string.Empty;
                                    return n.Name.Equals(name) && n.GroupKey.Equals(groupKey);
                                }
                                return false;
                            }
                        );
                    XmlLink link = new XmlLink();
                    if (existValue == null)
                    {
                        LinkGroupData linkGroup = new LinkGroupData();
                        linkGroup.Index = Convert.ToInt32(reader.GetAttribute("Index"));
                        linkGroup.Name = reader.GetAttribute("Name");
                        linkGroup.DisplayName = reader.GetAttribute("DisplayName");
                        linkGroup.IsTitleLink = Convert.ToBoolean(reader.GetAttribute("IsTitleLink"));
                        linkGroup.GroupKey = reader.GetAttribute("GroupKey") ?? string.Empty;
                        linkGroup.Assembly = extension.Assembly;
                        extension.ExtensionNodes.Add(linkGroup);
                        link.Setup(reader, ref linkGroup);
                    }
                    else
                    {
                        link.Setup(reader, ref existValue);
                    }

                    extension.ExtensionNodes.Sort((a, b) => a.Index.CompareTo(b.Index));
                }
            }
        }
    }
}
