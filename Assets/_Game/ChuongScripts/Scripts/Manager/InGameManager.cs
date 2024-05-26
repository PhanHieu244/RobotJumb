using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChuongCustom
{
    public class InGameManager : Singleton<InGameManager>
    {
        [SerializeField] public int PriceToPrice = 1;
        [SerializeField] private float loseRange = 2f;

        private void Start()
        {
            Time.timeScale = 0f;
            ScoreManager.Reset();
            Robot.posStand = Spawner.StandPos;
        }

        [Button]
        public void Win()
        {
            Manager.ScreenManager.OpenScreen(ScreenType.Result);
            //todo:
        }

        [Button]
        public void Lose()
        {
            Manager.ScreenManager.OpenScreen(ScreenType.Lose);
            //todo:
        }

        [Button]
        public void BeforeLose()
        {
            Manager.ScreenManager.OpenScreen(ScreenType.BeforeLose);
            //todo:
        }

        public void Retry()
        {
            //retry
            //todo:
            SceneManager.LoadScene("GameScene");
        }

        public void Continue()
        {
            //continue
            Revive();
            //todo:
        }

        private BaseMover _baseMover;
        private bool _isStop;

        public BaseMover BaseMover => _baseMover;

        private Spawner Spawner => global::Spawner.Instance;
        private Robot Robot => Robot.Instance;

        public void TapToStart()
        {
            Robot.OnStanding -= Spawn;
            Robot.OnStanding += Spawn;
            Robot.OnFailStanding -= ShowLose;
            Robot.OnFailStanding += ShowLose;
            Time.timeScale = 1f;
            _isStop = true;
            DOVirtual.DelayedCall(0.2f, () =>
            {
                Spawner.Spawn(out _baseMover);
                _isStop = false;
                Robot.Jump();
            });
        }

        public void Stop()
        {
            if (_isStop) return;
            _baseMover.DOComplete(true);
            _baseMover.Stop();
            _isStop = true;
            var deltaX = Mathf.Abs(_baseMover.transform.position.x);
            if (IsLose(deltaX))
            {
                Robot.FailDown();
                return;
            }

            var point = IsPerfect(deltaX) ? 5 : 1;
            _baseMover.ShowText(point).OnComplete((() =>
            {
                ScoreManager.Score += point;
            })).SetTarget(_baseMover);
            Robot.Down(Spawner.StandPos);
        }

        private void Spawn()
        {
            Spawner.MoveHigher(_baseMover);
            Spawner.Spawn(out _baseMover);
            Robot.Jump();
            _isStop = false;
        }
        
        public void Revive()
        {
            Time.timeScale = 1f;
            Destroy(_baseMover.gameObject);
            Spawner.Spawn(out _baseMover);
            Robot.Revive();
            Robot.Jump();
            _isStop = false;
            //todo reset robot
        }

        private void ShowLose()
        {
            Time.timeScale = 0f;
            BeforeLose();
        }

        private bool IsLose(float deltaX)
        {
            return deltaX >= loseRange;
        }

        private bool IsPerfect(float deltaX)
        {
            return deltaX <= 0.06f;
        }
    }
}