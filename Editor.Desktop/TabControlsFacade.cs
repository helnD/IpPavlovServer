using Editor.Desktop.Views.Common;

namespace Editor.Desktop
{
    /// <summary>
    /// Class for providing all  tab controls from main window.
    /// </summary>
    public class TabControlsFacade
    {
        public TabControlsFacade(
            Certificates certificates,
            Products products,
            SalesLeaders salesLeaders,
            SalesRepresentatives representatives,
            Categories categories)
        {
            Certificates = certificates;
            Products = products;
            SalesLeaders = salesLeaders;
            Representatives = representatives;
            Categories = categories;
        }

        /// <summary>
        /// Certificates tab control.
        /// </summary>
        internal Certificates Certificates { get; }

        /// <summary>
        /// Products tab control.
        /// </summary>
        internal Products Products { get; }

        /// <summary>
        /// Sales leaders tab control.
        /// </summary>
        internal SalesLeaders SalesLeaders { get; }

        /// <summary>
        /// Sales representatives tab control.
        /// </summary>
        internal SalesRepresentatives Representatives { get; }

        /// <summary>
        /// Categories = tab control.
        /// </summary>
        internal Categories Categories { get; }
    }
}