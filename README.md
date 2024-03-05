Unity 2020 or higher

❓Tutorial: https://www.youtube.com/watch?v=yyOzL2PgGfA

🔴Buymeacoffee: https://www.buymeacoffee.com/baltared

🔴Discord: https://discord.gg/unJ7aEh65R

Just import it into your project and follow the tutorial

🔹 PoolingManager or PoolingManagerStandard.HandlerInitialize(); ✶ Initializes the pooling system. Can be used in the Start or Awake method.

🔹 GameObject pool = PoolingManager or PoolingManagerStandard.HandlerOnGet(); ✶ Returns a gameobject pool.

🔹 PoolingManager or PoolingManagerStandard.HandlerRelease(); ✶ Returns a gameobject back in the pooling system.

🔹 PoolingManager or PoolingManagerStandard.HandlerReleaseAll(); ✶ Returns all gameobjects back into the pooling system.

🔹 PoolingManager or PoolingManagerStandard.AddListenerGet(); ✶ Event that is triggered when some gameobject pool is generated.

🔹 PoolingManager or PoolingManagerStandard.RemoveListenerGet(); ✶ Remove event method.

🔹 PoolingManager or PoolingManagerStandard.AddListenerRelease(); ✶ Event that is triggered when returning gameobject to the pooling system.

🔹 PoolingManager or PoolingManagerStandard.RemoveListenerRelease(); ✶ Remove event method.

🔷Create the scriptable object
![Media 05_03_2024 07-10-16](https://github.com/SamuelSatiroDev/Scriptableobject-Pooling-System/assets/107225086/2811eb44-d2de-4f5e-83f7-a157eb0eced2)

🔷Swap instantiate for pooling system
![Media 05_03_2024 07-11-58](https://github.com/SamuelSatiroDev/Scriptableobject-Pooling-System/assets/107225086/9024d181-1e5d-4cae-9985-a45fa9940bbd)
![Media 05_03_2024 07-13-13](https://github.com/SamuelSatiroDev/Scriptableobject-Pooling-System/assets/107225086/0c9ef8d4-d6dc-4d2e-83ad-8504e832d644)

🔷Result
![Media 05_03_2024 07-24-06](https://github.com/SamuelSatiroDev/Scriptableobject-Pooling-System/assets/107225086/f25bd137-de88-4ffc-9934-eee18773a51c)
