using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AICondition_HasNoticedPlayer : BaseAICondition
{
    [field:SerializeField]
    public AICondition_NoticePlayer Notice { get; private set; }
    public override bool CheckCondition()
    {
        return Notice.Noticed;
    }
}
