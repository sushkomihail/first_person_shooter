using System.Collections.Generic;
using Initializator;
using UnityEngine;

namespace Character.Avatar
{
    public class HumanAvatar : Initializable
    {
        [SerializeField] private List<AvatarConnection> _connections;
        [SerializeField] private Transform _breakParent;

        public override void Initialize()
        {
            SetConnectionParameters(AvatarActionType.Break);
            SetConnections();
            SetConnectionParameters(AvatarActionType.Connect);
        }

        public void SetBoneConnection(BoneType boneType)
        {
            AvatarConnection connection = FindConnection(boneType);
            connection?.Connect();
        }

        public void BreakBoneConnection(BoneType boneType)
        {
            AvatarConnection connection = FindConnection(boneType);
            connection?.Break(_breakParent);
        }

        public AvatarConnection FindConnection(BoneType boneType)
        {
            foreach (AvatarConnection connection in _connections)
            {
                if (connection.BoneType == boneType)
                {
                    return connection;
                }
            }

            return null;
        }

        private void SetConnectionParameters(AvatarActionType type)
        {
            foreach (AvatarConnection connection in _connections)
            {
                connection.SetParameters(type);
            }
        }
        
        private void SetConnections()
        {
            foreach (AvatarConnection connection in _connections)
            {
                connection.Connect();
            }
        }
    }
}