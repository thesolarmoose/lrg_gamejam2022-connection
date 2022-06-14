using System.Collections;

namespace Hook.RetractionControllers
{
    public interface IRetractionController
    {
        IEnumerator Retract(Connection first, Connection second);
    }
}