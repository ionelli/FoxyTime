used technologies

Arduino:
- 2x flex sensors, used to capture squeezing of plush animal (https://www.sparkfun.com/products/8606)
- 1x ultrasound distance sensor, used to capture distance between plush and player (https://www.sparkfun.com/products/15569)
- 1x motion sensor, used to capture when user starts moving in range of plush (https://www.sparkfun.com/products/13968) [NOT EXACT MATCH]
- JSON file format, used to transmit data to unity safely over serial (https://arduinojson.org/)

Unity:
- URP, used to enable advanced shader and VFX systems (https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0)
- Navmesh package, used to simplify pathfinding for fox (https://github.com/Unity-Technologies/NavMeshComponents)
- Blend tree animations, used to allow fox to move and act in a believable way (https://docs.unity3d.com/Manual/class-BlendTree.html)
- Oculus quest 2, main interface for audiovisual output and player input through controllers (https://www.oculus.com/quest-2/)