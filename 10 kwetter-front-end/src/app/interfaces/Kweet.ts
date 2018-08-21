import { Like } from "./Like";
import { User } from "./User";

export interface Kweet {
    isLiked: boolean,
    likesCount: number,
    user: User,
    id: number,
    description: string,
    date: Date 
}