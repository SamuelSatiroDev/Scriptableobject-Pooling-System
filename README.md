<h1>ScriptableObject Pooling System</h1>
<br>
<h3>Unity 2020 or higher</h3>
<br>
❓ Tutorial: https://www.youtube.com/watch?v=yyOzL2PgGfA <br>
📌 Buymeacoffee: https://www.buymeacoffee.com/baltared <br>
📌 Discord: https://discord.gg/unJ7aEh65R<br>
<br>
<br>
<br>
<b>Just import it into your project and follow the tutorial</b><br>
<br>
🔹 <b>Initializes the pooling system. Can be used in the Start or Awake method:</b><br>
<i>(Unity 2021 or higher)</i>PoolingManager.HandlerInitialize()<br>
<i>(Unity 2020)</i>PoolingManagerStandard.HandlerInitialize()<br>
<br>
<br>
🔹 <b>GameObject pool. Returns a gameobject pool:</b><br>
<i>(Unity 2021 or higher)</i>PoolingManager.HandlerOnGet()<br>
<i>(Unity 2020)</i>PoolingManagerStandard.HandlerOnGet()<br>
<br>
<br>
🔹<b>Returns a gameobject back in the pooling system:</b><br>
<i>(Unity 2021 or higher)</i>PoolingManager.HandlerRelease()<br>
<i>(Unity 2020)</i>PoolingManagerStandard.HandlerRelease() <br>
<br>
<br>
🔹 <b>Returns all gameobjects back into the pooling system:</b><br>
<i>(Unity 2021 or higher)</i>PoolingManager.HandlerReleaseAll()<br>
<i>(Unity 2020)</i>PoolingManagerStandard.HandlerReleaseAll()<br>
<br>
<br>
🔹 <b>Event that is triggered when some gameobject pool is generated:</b><br>
Add<br>
<i>(Unity 2021 or higher)</i>PoolingManager.AddListenerGet()<br>
<i>(Unity 2020)</i>PoolingManagerStandard.AddListenerGet()<br>
<br>
Remove<br>
<i>(Unity 2021 or higher)</i>PoolingManager.RemoveListenerGet()<br>
<i>(Unity 2020)</i>PoolingManagerStandard.RemoveListenerGet()<br>
<br>
<br>
🔹 <b>Event that is triggered when returning gameobject to the pooling system:</b><br>
Add<br>
<i>(Unity 2021 or higher)</i>PoolingManager.AddListenerRelease()<br>
<i>(Unity 2020)</i>PoolingManagerStandard.AddListenerRelease()<br>
<br>
Remove<br>
<i>(Unity 2021 or higher)</i>PoolingManager.RemoveListenerRelease()<br>
<i>(Unity 2020)</i>PoolingManagerStandard.RemoveListenerRelease()<br>

🔷Create the scriptable object
![Media 05_03_2024 07-10-16](https://github.com/SamuelSatiroDev/Scriptableobject-Pooling-System/assets/107225086/2811eb44-d2de-4f5e-83f7-a157eb0eced2)

🔷Swap instantiate for pooling system
![Media 05_03_2024 07-11-58](https://github.com/SamuelSatiroDev/Scriptableobject-Pooling-System/assets/107225086/9024d181-1e5d-4cae-9985-a45fa9940bbd)
![Media 05_03_2024 07-13-13](https://github.com/SamuelSatiroDev/Scriptableobject-Pooling-System/assets/107225086/0c9ef8d4-d6dc-4d2e-83ad-8504e832d644)

🔷Result
![Media 05_03_2024 07-24-06](https://github.com/SamuelSatiroDev/Scriptableobject-Pooling-System/assets/107225086/f25bd137-de88-4ffc-9934-eee18773a51c)
