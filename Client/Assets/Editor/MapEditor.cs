using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

// collision 데이터를 txt 파일로 추출해주는 Map관련 Tool 코드

public class MapEditor 
{

    // 해당 코드 이하는 프로덕션 모드에선 사용되지 않음
    #if UNITY_EDITOR

    //단축키 지정 => % (Ctrl), # (Shift), & (Alt)
    [MenuItem("Tools/GenerateMap %#g")]
	private static void GenerateMap()
	{
        GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (GameObject go in gameObjects)
		{
        Tilemap tm = Util.FindChild<Tilemap>(go, "Tilemap_Collision", true);

        // 서버에 전달할 titlemap collision data 생성 (txt 파일)
        using (var writer = File.CreateText($"Assets/Resources/Map/{go.name}.txt"))
        {
            // 지도의 상하좌우 크기 출력
            writer.WriteLine(tm.cellBounds.xMin);
            writer.WriteLine(tm.cellBounds.yMin);
            writer.WriteLine(tm.cellBounds.xMax);
            writer.WriteLine(tm.cellBounds.yMax);

            /*
            0과 1로 접근가능좌표/접근불가 좌표 출력
            예)
            0000001110010101
            0000001110010100
            0000100001010010
            0000110010100101
            */

            for(int y = tm.cellBounds.yMax; y>= tm.cellBounds.yMin; y--){
                for(int x = tm.cellBounds.xMin; x <= tm.cellBounds.xMax; x++){
                    TileBase tile = tm.GetTile(new Vector3Int(x, y, 0));
                    if(tile != null){
                        writer.Write("1");
                    }else{
                        writer.Write("0");
                    }
                }
                writer.WriteLine();
            }

        }
        }

    }

    #endif
}
