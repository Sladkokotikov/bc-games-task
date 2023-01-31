using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

namespace DrawAndRun
{
    public class LevelInitializer : MonoBehaviour
    {
        private SplineComputer _computer;
        [SerializeField] private List<ColorMapping> mappings;
        [SerializeField] private float childScale;
        [SerializeField] private float childRange;
        [SerializeField] private float pointOffset;
        private Dictionary<Color, GameObject> _resources;

        private int _levelWidth, _levelHeight;

        private void Awake()
        {
            _resources = new Dictionary<Color, GameObject>();
            foreach (var map in mappings)
                _resources[map.color] = map.prefab;
        }

        public void InitializeLevel(LevelInfo level)
        {
            _computer = level.computer;
            _computer.gameObject.SetActive(true);
            for (var i = 0; i < _computer.pointCount; i++)
            {
                var point = _computer.GetPoint(i);
                point.position +=
                    pointOffset * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                _computer.SetPoint(i, point);
            }

            _levelWidth = level.texture.width;
            _levelHeight = level.texture.height;
            var colors = new Dictionary<Color, List<Vector2Int>>();
            for (var i = 0; i < _levelWidth; i++)
            {
                for (var j = 0; j < _levelHeight; j++)
                {
                    var color = level.texture.GetPixel(i, j);
                    if (!colors.ContainsKey(color))
                        colors[color] = new List<Vector2Int>();
                    colors[color].Add(new Vector2Int(i, j));
                }
            }

            SpawnLevel(colors);
        }

        private void SpawnLevel(Dictionary<Color, List<Vector2Int>> colors)
        {
            var length = _computer.CalculateLength();
            foreach (var k in colors.Keys.Where(_resources.ContainsKey))
            {
                foreach (var pos in colors[k])
                {
                    var percent = (float) pos.y / _levelHeight;
                    var sample = _computer.Evaluate(_computer.Travel(0, length * percent));
                    var x = (_levelWidth % 2 == 0 ? -0.5f : 0) - _levelWidth / 2 + pos.x;
                    var obj = Instantiate(_resources[k],
                        sample.position + childRange * x * sample.right, sample.rotation);
                    obj.transform.localScale *= childScale;
                }
            }
        }
    }
}