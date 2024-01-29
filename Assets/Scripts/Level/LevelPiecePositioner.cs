using System.Threading.Tasks;
using UnityEngine;

public class LevelPiecePositioner : MonoBehaviour
{
    public async void PosePieces()
    {
        for( int i = 1; i < transform.childCount; i++ )
        {
            if(transform.GetChild(i - 1).TryGetComponent(out LevelPiece piece))
            {
                var splineSample = piece.MainSpline.Evaluate(piece.MainSpline.pointCount);
                transform.GetChild(i).position = splineSample.position;
                transform.GetChild(i).rotation = Quaternion.LookRotation(splineSample.forward);
                await Task.Yield();
            }
            await Task.Yield();
        }
    }
}
