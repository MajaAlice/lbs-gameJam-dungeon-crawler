using System.Collections.Generic;
using UnityEngine;

public class AtomicHandler : MonoBehaviour
{
    public float ProtonMass = 1.0f;
    public float NeutornMass = 1.0f;
    public float ElectronMass = 1.0f;

    public float MaxStrongForceDistance = 2.5f;
    public float TimeStep = 1.0f;

    public float GravitationalConstant = 1.0f;
    public float CoulombsConstant = 1.0f;
    public float StrongForceConstant = 1.0f;

    public float MaxParticleDistance = 1.0f;

    public enum ParticleType {None,Proton,Neutron,Electron};
    public class Particle
    {
        public ParticleType Type;
        public Vector3 Position;
        public Vector3 Velocity;
        public Particle(ParticleType type, Vector3 position, Vector3 velocity)
        {
            Type = type;
            Position = position;
            Velocity = velocity;
        }
    }
    public List<Particle> Particles = new List<Particle>();
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnParticle(ParticleType.Proton);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnParticle(ParticleType.Neutron);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnParticle(ParticleType.Electron);
        }
    }
    public void FixedUpdate()
    {
        for (int i = 0; i < Particles.Count; i++)
        {
            Particle ParticleA = Particles[i];

            float MassA = GetMass(ParticleA);
            float ChargeA = GetCharge(ParticleA);

            Vector3 TotalForce = Vector3.zero;

            for (int j = 0; j < Particles.Count; j++)
            {
                if (i == j) continue;

                Particle ParticleB = Particles[j];

                float MassB = GetMass(ParticleB);
                float ChargeB = GetCharge(ParticleB);

                Vector3 Direction = ParticleB.Position - ParticleA.Position;
                float Distance = Direction.magnitude;
                Direction = Direction.normalized;


                float GravityForce = GravitationalConstant * (MassA * MassB / Mathf.Pow(Distance, 2));
                TotalForce += Direction * GravityForce;

                if (ChargeA != 0 && ChargeB != 0)
                {
                    float CoulombsForce = CoulombsConstant * (ChargeA * ChargeB / Mathf.Pow(Distance,2));
                    TotalForce += Direction * -CoulombsForce;
                }

                if (Distance < MaxStrongForceDistance && ChargeA >= 0 && ChargeB >= 0)
                {
                    float StrongForce = StrongForceConstant * MassA * MassB / Mathf.Pow(Distance, 3);
                    TotalForce += Direction * StrongForce;
                }
            }
            ParticleA.Velocity += TotalForce / GetMass(ParticleA);
        }

        foreach(var ParticleA in Particles)
        {
            float RadiusA = GetSize(ParticleA);
            float MassA = GetMass(ParticleA);

            foreach (var ParticleB in Particles)
            {
                if (ParticleA == ParticleB) continue;

                Vector3 Direction = ParticleB.Position - ParticleA.Position;
                float Distance = Direction.magnitude;
                Direction = Direction.normalized;

                float RadiusB = GetSize(ParticleB);
                float MassB = GetMass(ParticleB);

                if (Distance <= RadiusA + RadiusB)
                {
                    Vector3 ReletiveVelocity = ParticleA.Velocity - ParticleB.Velocity;
                    float VelocityAlongDirection = Vector3.Dot(ReletiveVelocity, Direction);
                    if (VelocityAlongDirection < 0) continue;

                    float ImpulseMagnitude = -1f * VelocityAlongDirection / (1 / MassA + 1 / MassB);
                    Vector3 Impulse = ImpulseMagnitude * Direction;
                    ParticleA.Velocity += Impulse / MassA;
                    ParticleB.Velocity -= Impulse / MassB;

                    float OverLap = RadiusA + RadiusB - Distance;
                    ParticleA.Position -= Direction * (OverLap / 2);
                    ParticleB.Position += Direction * (OverLap / 2);
                }
            }
        }
        foreach(var Particle in Particles)
        {
            Particle.Position += Particle.Velocity * TimeStep;

            if (Particle.Position.magnitude > MaxParticleDistance)
            {
                Particle.Velocity = -Particle.Velocity * 0.9f;
                Particle.Position = Particle.Position.normalized * MaxParticleDistance;
            }
            if (Particle.Position.magnitude < -MaxParticleDistance)
            {
                Particle.Velocity = -Particle.Velocity * 0.9f;
                Particle.Position = Particle.Position.normalized * -MaxParticleDistance;
            }
        }
    }
    public void SpawnParticle(ParticleType type)
    {
        Vector3 MousePostion = Input.mousePosition;
        Vector3 WorldPostion = Camera.main.ScreenToWorldPoint(MousePostion);
        WorldPostion.z = Random.value;
        Particles.Add(new Particle(type, WorldPostion, Vector3.zero));
    }

    public float GetMass(Particle Particle)
    {
        return Particle.Type switch
        {
            ParticleType.Proton => ProtonMass,
            ParticleType.Neutron => NeutornMass,
            ParticleType.Electron => ElectronMass,
            _ => 1f
        };
    }
    public float GetCharge(Particle Particle)
    {
        return Particle.Type switch
        {
            ParticleType.Proton => 1,
            ParticleType.Neutron => 0,
            ParticleType.Electron => -1,
            _ => 1f
        };
    }
    public float GetSize(Particle Particle)
    {
         return Particle.Type switch
         {
             ParticleType.Proton => 0.5f,
             ParticleType.Neutron => 0.5f,
             ParticleType.Electron => 0.1f,
             _ => 1,
         };
    }
    private void OnDrawGizmos()
    {
        foreach (var Particle in Particles)
        {
            Gizmos.color = Particle.Type switch
            {
                ParticleType.Proton => Color.red,
                ParticleType.Neutron => Color.yellow,
                ParticleType.Electron => Color.cyan,
                _ => Color.green,
            };
            float Size = GetSize(Particle);
            Gizmos.DrawWireSphere(Particle.Position, Size);
        }
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(Vector3.zero, MaxParticleDistance);
    }
}