
namespace YPPGUtilities.Core
{
    /// <summary>
    /// The IRenderable class is used to provide GameObjects with the ability to be rendered.
    /// It adds a single method "Render" that accepts no parameters.
    /// </summary>
    public interface IRenderable
    {

        /// <summary>
        /// Renders the GameObject
        /// </summary>
        void Render();

    }

}
