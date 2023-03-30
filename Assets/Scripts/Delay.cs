using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Delay : MonoBehaviour
{
    [SerializeField] float Duration;
    // Start is called before the first frame update
     void Start()
    {
        
    }

    public async void Wait(){
        await Task.Delay((int)(Duration*1000));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
