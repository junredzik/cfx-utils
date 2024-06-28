using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace CsClient
{
    public static class Chub
    {
        public struct ClosestEntityResult<T> where T : Entity
        {
            public T ClosestEntity;
            public Vector3 ClosestCoords;
        }

        public static ClosestEntityResult<T> GetClosestEntity<T>(Vector3 coords, float maxDistance = 2.0f) where T : Entity
        {
            T closestEntity = null;
            Vector3 closestCoords = Vector3.Zero;
            float closestDistanceSquared = maxDistance * maxDistance;

            var entities = typeof(T) == typeof(Ped) ? World.GetAllPeds().Cast<T>() :
                           typeof(T) == typeof(Vehicle) ? World.GetAllVehicles().Cast<T>() :
                           typeof(T) == typeof(Prop) ? World.GetAllProps().Cast<T>() :
                           Enumerable.Empty<T>();

            foreach (var entity in entities)
            {
                var distanceSquared = coords.DistanceToSquared(entity.Position);
                if (distanceSquared < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquared;
                    closestEntity = entity;
                    closestCoords = entity.Position;
                }
            }

            return new ClosestEntityResult<T> { ClosestEntity = closestEntity, ClosestCoords = closestCoords };
        }
    }

    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            Vector3 myPosition = Game.PlayerPed.Position;

            var closestPedResult = Chub.GetClosestEntity<Ped>(myPosition, 100.0f);
            Debug.WriteLine(closestPedResult.ClosestEntity != null ?
                $"Najbliższy pieszy jest w odległości: {Vector3.Distance(closestPedResult.ClosestCoords, myPosition)}" :
                "Nie znaleziono pieszego w pobliżu.");

            var closestVehicleResult = Chub.GetClosestEntity<Vehicle>(myPosition, 100.0f);
            Debug.WriteLine(closestVehicleResult.ClosestEntity != null ?
                $"Najbliższy pojazd jest w odległości: {Vector3.Distance(closestVehicleResult.ClosestCoords, myPosition)}" :
                "Nie znaleziono pojazdu w pobliżu.");

            var closestObjectResult = Chub.GetClosestEntity<Prop>(myPosition, 100.0f);
            Debug.WriteLine(closestObjectResult.ClosestEntity != null ?
                $"Najbliższy obiekt jest w odległości: {Vector3.Distance(closestObjectResult.ClosestCoords, myPosition)}" :
                "Nie znaleziono obiektu w pobliżu.");
        }
    }
}