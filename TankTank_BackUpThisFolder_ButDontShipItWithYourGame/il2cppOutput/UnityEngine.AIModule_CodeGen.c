#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Boolean UnityEngine.AI.NavMeshAgent::SetDestination(UnityEngine.Vector3)
extern void NavMeshAgent_SetDestination_mD5D960933827F1F14B29CF4A3B6F305C064EBF46 (void);
// 0x00000002 System.Void UnityEngine.AI.NavMeshAgent::set_isStopped(System.Boolean)
extern void NavMeshAgent_set_isStopped_mF374E697F39845233B84D8C4873DEABC3AA490DF (void);
// 0x00000003 System.Boolean UnityEngine.AI.NavMeshAgent::SetDestination_Injected(UnityEngine.Vector3&)
extern void NavMeshAgent_SetDestination_Injected_mC3EF405F5AAFF9F98C5D5AECAD641525CDF742EA (void);
// 0x00000004 UnityEngine.Vector3 UnityEngine.AI.NavMeshHit::get_position()
extern void NavMeshHit_get_position_m09E8FF6DEF5BFA3F30B3C4BCA4642442FF1BCBF1 (void);
// 0x00000005 System.Void UnityEngine.AI.NavMesh::Internal_CallOnNavMeshPreUpdate()
extern void NavMesh_Internal_CallOnNavMeshPreUpdate_m80148CFDD0C6F1DDDE5B3DA67A8D9613043A4233 (void);
// 0x00000006 System.Boolean UnityEngine.AI.NavMesh::SamplePosition(UnityEngine.Vector3,UnityEngine.AI.NavMeshHit&,System.Single,System.Int32)
extern void NavMesh_SamplePosition_m51497866E71DD5263425E8572F2232D496E8F65A (void);
// 0x00000007 System.Boolean UnityEngine.AI.NavMesh::SamplePosition_Injected(UnityEngine.Vector3&,UnityEngine.AI.NavMeshHit&,System.Single,System.Int32)
extern void NavMesh_SamplePosition_Injected_m59777E11E947B46F60E9BCDF8C7CCFFAA5529E0D (void);
// 0x00000008 System.Void UnityEngine.AI.NavMesh/OnNavMeshPreUpdate::.ctor(System.Object,System.IntPtr)
extern void OnNavMeshPreUpdate__ctor_m7142A3AA991BE50B637A16D946AB7604C64EF9BA (void);
// 0x00000009 System.Void UnityEngine.AI.NavMesh/OnNavMeshPreUpdate::Invoke()
extern void OnNavMeshPreUpdate_Invoke_mFB224B9BBF9C78B7F39AA91A047F175C69897914 (void);
static Il2CppMethodPointer s_methodPointers[9] = 
{
	NavMeshAgent_SetDestination_mD5D960933827F1F14B29CF4A3B6F305C064EBF46,
	NavMeshAgent_set_isStopped_mF374E697F39845233B84D8C4873DEABC3AA490DF,
	NavMeshAgent_SetDestination_Injected_mC3EF405F5AAFF9F98C5D5AECAD641525CDF742EA,
	NavMeshHit_get_position_m09E8FF6DEF5BFA3F30B3C4BCA4642442FF1BCBF1,
	NavMesh_Internal_CallOnNavMeshPreUpdate_m80148CFDD0C6F1DDDE5B3DA67A8D9613043A4233,
	NavMesh_SamplePosition_m51497866E71DD5263425E8572F2232D496E8F65A,
	NavMesh_SamplePosition_Injected_m59777E11E947B46F60E9BCDF8C7CCFFAA5529E0D,
	OnNavMeshPreUpdate__ctor_m7142A3AA991BE50B637A16D946AB7604C64EF9BA,
	OnNavMeshPreUpdate_Invoke_mFB224B9BBF9C78B7F39AA91A047F175C69897914,
};
extern void NavMeshHit_get_position_m09E8FF6DEF5BFA3F30B3C4BCA4642442FF1BCBF1_AdjustorThunk (void);
static Il2CppTokenAdjustorThunkPair s_adjustorThunks[1] = 
{
	{ 0x06000004, NavMeshHit_get_position_m09E8FF6DEF5BFA3F30B3C4BCA4642442FF1BCBF1_AdjustorThunk },
};
static const int32_t s_InvokerIndices[9] = 
{
	2171,
	2879,
	2007,
	3616,
	5541,
	4089,
	4055,
	1631,
	3624,
};
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_AIModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_AIModule_CodeGenModule = 
{
	"UnityEngine.AIModule.dll",
	9,
	s_methodPointers,
	1,
	s_adjustorThunks,
	s_InvokerIndices,
	0,
	NULL,
	0,
	NULL,
	0,
	NULL,
	NULL,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};
