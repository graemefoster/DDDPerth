using System.Collections.Generic;

namespace Pipline.Testing
{
    public static class ScenarioEx 
    {
        public static IEnumerable<IScenario> And(this IScenario scenario1, IScenario scenario2) {
            yield return scenario1;
            yield return scenario2;
        }
        public static IEnumerable<IScenario> And(this IEnumerable<IScenario> scenarios, IScenario nextScenario) {
            foreach (var scenario in scenarios)
            {
                yield return scenario;
            }
            yield return nextScenario;
        }
    }
}
