export interface Goal{
    id: string,
    title: string,
    description: string | null,
    createdAt: string,
    deadline: string | null
}
export type GoalRequest = Omit<Goal, 'id' | 'createdAt'>;