using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
using UnityEngine;

namespace Assets.Scripts.Character.Player
{
    public class RecenterOnPositionError : BaseGameObject
    {
        private PlayerIdentifier _player;

        protected override void OnAwake()
        {
            base.OnAwake();
            _player = _player.FromScene();
        }

        private void Update()
        {
            if (_player.CurrentRoom == null) return;
            if (Vector2.Distance(_player.CurrentRoom.transform.position, _player.transform.position) > 30)
            {
                _player.transform.position = _player.CurrentRoom.transform.position;
            }
        }
    }
}
