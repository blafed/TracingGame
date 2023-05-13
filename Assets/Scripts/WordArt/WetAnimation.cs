using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace KidLetters.WordArt
{
    public class WetAnimation : WordArtAnimation
    {
        [System.Serializable]
        public class Droplet
        {
            public GameObject prefab;
            public Rect spawnArea = new Rect(0, 0, 1, 1);
            public float duration = 1;
            public float rate = .5f;
            public MinMaxF range = new MinMaxF(3, 6);
        }

        public Droplet droplet;
        public float maxSpawningTime = 20;


        float nextSpawnTime;
        float startSpawnTime;





        private void Start()
        {
            droplet.prefab.SetActive(false);
        }


        public override void playAnimation()
        {
            base.playAnimation();
            startSpawnTime = Time.time;
        }
        private void Update()
        {
            if (!animationBegan)
                return;
            if (Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + droplet.rate;
                var obj = Instantiate(droplet.prefab, transform);
                obj.SetActive(true);
                obj.transform.localPosition = new Vector3(
                    Random.Range(droplet.spawnArea.xMin, droplet.spawnArea.xMax),
                    Random.Range(droplet.spawnArea.yMin, droplet.spawnArea.yMax),
                    0);
                obj.transform.DOMove(obj.transform.position + Vector3.down * droplet.range.random, droplet.duration).SetEase(Ease.InQuad);
                obj.GetComponent<SpriteRenderer>().DOFade(0, droplet.duration).SetEase(Ease.InQuad);
            }

            if (Time.time - startSpawnTime > maxSpawningTime)
                enabled = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + new Vector3(droplet.spawnArea.center.x, droplet.spawnArea.center.y, 0), new Vector3(droplet.spawnArea.width, droplet.spawnArea.height, 0));
        }
    }
}